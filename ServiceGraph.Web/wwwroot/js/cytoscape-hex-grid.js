/*!
 * Cytoscape Hexagonal Grid Extension
 * A Cytoscape.js extension for hexagonal grid background with node snapping
 */

(function () {
    'use strict';

    // Default options
    var defaults = {
        // Hexagon grid options
        hexSize: 30,                    // Size of hexagons (radius)
        hexColor: '#007aff',           // Color of hex grid lines
        hexLineWidth: 1.0,             // Width of hex grid lines
        showDots: true,                // Show dots at intersections
        dotColor: '#007aff',           // Color of intersection dots
        dotSize: 2,                    // Size of intersection dots
        hexOpacity: 0.3,               // Opacity of hex grid
        dotOpacity: 0.6,               // Opacity of dots
        
        // Grid behavior
        drawHexGrid: true,             // Draw hexagonal grid background
        snapToHex: true,               // Snap nodes to hex grid
        snapToHexCenter: true,         // Snap to hex centers (false = snap to vertices)
        hexStackOrder: -1,             // Z-index of hex grid
        
        // Zoom and pan behavior
        zoomDash: true,                // Scale grid with zoom
        panGrid: false,                // Move grid with pan
        
        // Performance
        redrawOnZoom: true,
        redrawOnPan: false
    };

    // Hexagon math utilities
    var HexMath = {
        // Convert hex coordinates to pixel coordinates (proper honeycomb layout)
        hexToPixel: function(hexX, hexY, hexSize) {
            var x = hexSize * (3/2 * hexX);
            var y = hexSize * (Math.sqrt(3)/2 * hexX + Math.sqrt(3) * hexY);
            return { x: x, y: y };
        },
        
        // Convert pixel coordinates to hex coordinates (proper honeycomb layout)
        pixelToHex: function(x, y, hexSize) {
            var q = (2/3 * x) / hexSize;
            var r = (-1/3 * x + Math.sqrt(3)/3 * y) / hexSize;
            return this.roundHex(q, r);
        },
        
        // Round fractional hex coordinates to nearest hex
        roundHex: function(q, r) {
            var s = -q - r;
            var rq = Math.round(q);
            var rr = Math.round(r);
            var rs = Math.round(s);
            
            var qDiff = Math.abs(rq - q);
            var rDiff = Math.abs(rr - r);
            var sDiff = Math.abs(rs - s);
            
            if (qDiff > rDiff && qDiff > sDiff) {
                rq = -rr - rs;
            } else if (rDiff > sDiff) {
                rr = -rq - rs;
            }
            
            return { q: rq, r: rr };
        },
        
        // Get hex vertices in pixel coordinates (flat-top orientation)
        getHexVertices: function(centerX, centerY, hexSize) {
            var vertices = [];
            for (var i = 0; i < 6; i++) {
                var angleDeg = 60 * i; // No offset for flat-top orientation
                var angleRad = Math.PI / 180 * angleDeg;
                var x = centerX + hexSize * Math.cos(angleRad);
                var y = centerY + hexSize * Math.sin(angleRad);
                vertices.push({ x: x, y: y });
            }
            return vertices;
        }
    };

    // Main hex grid implementation
    function createHexGrid(cy, options) {
        var container = cy.container();
        var canvas = document.createElement('canvas');
        var ctx = canvas.getContext('2d');
        
        // Setup canvas
        canvas.style.position = 'absolute';
        canvas.style.top = '0';
        canvas.style.left = '0';
        canvas.style.pointerEvents = 'none';
        canvas.style.zIndex = options.hexStackOrder;
        container.appendChild(canvas);
        
        // Utility functions
        function resizeCanvas() {
            canvas.width = cy.width();
            canvas.height = cy.height();
            if (options.drawHexGrid) {
                drawHexGrid();
            }
        }
        
        function clearCanvas() {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
        }
        
        function resetCanvas() {
            canvas.width = 0;
            canvas.height = 0;
            canvas.style.position = 'absolute';
            canvas.style.top = '0';
            canvas.style.left = '0';
            canvas.style.zIndex = options.hexStackOrder;
        }
        
        // Draw hexagonal grid
        function drawHexGrid() {
            clearCanvas();
            
            if (!options.drawHexGrid) return;
            
            var zoom = options.zoomDash ? cy.zoom() : 1;
            var pan = cy.pan();
            var hexSize = options.hexSize * zoom;
            var width = canvas.width;
            var height = canvas.height;
            
            // Calculate grid offset based on pan
            var offsetX = options.panGrid ? pan.x : 0;
            var offsetY = options.panGrid ? pan.y : 0;
            
            // Calculate proper hexagonal tiling dimensions for edge-sharing honeycomb (flat-top)
            var hexWidth = hexSize * Math.sqrt(3);
            var hexHeight = hexSize * 2;
            var horizontalSpacing = hexSize * 3/2;  // Distance between hex centers horizontally
            var verticalSpacing = hexSize * Math.sqrt(3);  // Distance between hex centers vertically
            
            // Calculate grid bounds with proper hexagonal tiling
            var startCol = Math.floor((-offsetX - hexSize) / horizontalSpacing) - 1;
            var endCol = Math.ceil((width - offsetX + hexSize) / horizontalSpacing) + 1;
            var startRow = Math.floor((-offsetY - hexSize) / verticalSpacing) - 1;
            var endRow = Math.ceil((height - offsetY + hexSize) / verticalSpacing) + 1;
            
            // Store intersection points for dots
            var intersections = [];
            
            // Draw hexagons
            ctx.strokeStyle = options.hexColor;
            ctx.lineWidth = options.hexLineWidth;
            ctx.globalAlpha = options.hexOpacity;
            
            for (var col = startCol; col <= endCol; col++) {
                for (var row = startRow; row <= endRow; row++) {
                    // Calculate hex center position for proper honeycomb tiling
                    var centerX = col * horizontalSpacing + offsetX;
                    var centerY = row * verticalSpacing + offsetY;
                    
                    // Offset every other column for proper hex tiling (flat-top hexagons)
                    if (col % 2 !== 0) {
                        centerY += verticalSpacing / 2;
                    }
                    
                    // Skip if hex is completely outside canvas
                    if (centerX + hexSize < 0 || centerX - hexSize > width ||
                        centerY + hexSize < 0 || centerY - hexSize > height) {
                        continue;
                    }
                    
                    // Get hex vertices
                    var vertices = HexMath.getHexVertices(centerX, centerY, hexSize);
                    
                    // Draw hexagon
                    ctx.beginPath();
                    ctx.moveTo(vertices[0].x, vertices[0].y);
                    for (var i = 1; i < vertices.length; i++) {
                        ctx.lineTo(vertices[i].x, vertices[i].y);
                    }
                    ctx.closePath();
                    ctx.stroke();
                    
                    // Store intersection points (vertices)
                    if (options.showDots) {
                        vertices.forEach(function(vertex) {
                            intersections.push(vertex);
                        });
                    }
                }
            }
            
            // Draw intersection dots
            if (options.showDots && intersections.length > 0) {
                ctx.fillStyle = options.dotColor;
                ctx.globalAlpha = options.dotOpacity;
                
                // Remove duplicate intersections (approximate)
                var uniqueIntersections = [];
                var tolerance = options.dotSize;
                
                intersections.forEach(function(point) {
                    var isDuplicate = uniqueIntersections.some(function(existing) {
                        return Math.abs(existing.x - point.x) < tolerance &&
                               Math.abs(existing.y - point.y) < tolerance;
                    });
                    
                    if (!isDuplicate) {
                        uniqueIntersections.push(point);
                    }
                });
                
                // Draw dots
                uniqueIntersections.forEach(function(point) {
                    if (point.x >= -options.dotSize && point.x <= width + options.dotSize &&
                        point.y >= -options.dotSize && point.y <= height + options.dotSize) {
                        ctx.beginPath();
                        ctx.arc(point.x, point.y, options.dotSize, 0, 2 * Math.PI);
                        ctx.fill();
                    }
                });
            }
            
            ctx.globalAlpha = 1; // Reset alpha
        }
        
        // Snap node to hex grid
        function snapToHexGrid(node) {
            if (!options.snapToHex) return;
            
            var pos = node.position();
            var zoom = cy.zoom();
            var pan = cy.pan();
            
            // Convert to grid space
            var gridX = (pos.x - pan.x) / zoom;
            var gridY = (pos.y - pan.y) / zoom;
            
            // Find nearest hex
            var hex = HexMath.pixelToHex(gridX, gridY, options.hexSize);
            var snapPos;
            
            if (options.snapToHexCenter) {
                // Snap to hex center
                snapPos = HexMath.hexToPixel(hex.q, hex.r, options.hexSize);
            } else {
                // Snap to nearest vertex
                var center = HexMath.hexToPixel(hex.q, hex.r, options.hexSize);
                var vertices = HexMath.getHexVertices(center.x, center.y, options.hexSize);
                
                var minDist = Infinity;
                snapPos = center;
                
                vertices.forEach(function(vertex) {
                    var dist = Math.sqrt(Math.pow(vertex.x - gridX, 2) + Math.pow(vertex.y - gridY, 2));
                    if (dist < minDist) {
                        minDist = dist;
                        snapPos = vertex;
                    }
                });
            }
            
            // Convert back to world space
            var worldX = snapPos.x * zoom + pan.x;
            var worldY = snapPos.y * zoom + pan.y;
            
            return { x: worldX, y: worldY };
        }
        
        // Event handlers
        function onZoom() {
            if (options.redrawOnZoom) {
                drawHexGrid();
            }
        }
        
        function onPan() {
            if (options.redrawOnPan) {
                drawHexGrid();
            }
        }
        
        function onResize() {
            resizeCanvas();
        }
        
        // API object
        var api = {
            drawHexGrid: drawHexGrid,
            clearCanvas: clearCanvas,
            resetCanvas: resetCanvas,
            resizeCanvas: resizeCanvas,
            snapToHexGrid: snapToHexGrid,
            
            enable: function() {
                resizeCanvas();
                cy.on('zoom', onZoom);
                cy.on('pan', onPan);
                cy.on('resize', onResize);
            },
            
            disable: function() {
                clearCanvas();
                resetCanvas();
                cy.off('zoom', onZoom);
                cy.off('pan', onPan);
                cy.off('resize', onResize);
            },
            
            updateOptions: function(newOptions) {
                if (newOptions) {
                    for (var key in newOptions) {
                        options[key] = newOptions[key];
                    }
                }
                if (options.drawHexGrid) {
                    drawHexGrid();
                } else {
                    clearCanvas();
                }
            },
            
            destroy: function() {
                this.disable();
                if (canvas.parentNode) {
                    canvas.parentNode.removeChild(canvas);
                }
            }
        };
        
        return api;
    }

    // Register the extension
    function register(cytoscape) {
        if (!cytoscape) {
            console.warn('Cytoscape not available for hex grid extension');
            return;
        }
        
        try {
            cytoscape('core', 'hexGrid', function(opts) {
                var cy = this;
                var options = {};
                
                // Safely merge options
                for (var key in defaults) {
                    options[key] = defaults[key];
                }
                if (opts) {
                    for (var key in opts) {
                        options[key] = opts[key];
                    }
                }
                
                try {
                    // Create or get existing instance
                    var scratch = cy.scratch();
                    if (!scratch.hexGrid) {
                        scratch.hexGrid = createHexGrid(cy, options);
                        
                        if (options.drawHexGrid) {
                            scratch.hexGrid.enable();
                        }
                    } else {
                        scratch.hexGrid.updateOptions(options);
                    }
                    
                    return scratch.hexGrid;
                } catch (error) {
                    console.error('Error creating hex grid:', error);
                    return null;
                }
            });
        } catch (error) {
            console.error('Error registering hex grid extension:', error);
        }
    }
    
    // Export for different module systems
    if (typeof module !== 'undefined' && module.exports) {
        module.exports = register;
    }
    
    if (typeof define !== 'undefined' && define.amd) {
        define(function() {
            return register;
        });
    }
    
    // Single registration check - only register once
    if (typeof cytoscape !== 'undefined' && typeof window !== 'undefined') {
        try {
            register(cytoscape);
        } catch (error) {
            console.error('Error registering hex grid extension:', error);
        }
    }
})();