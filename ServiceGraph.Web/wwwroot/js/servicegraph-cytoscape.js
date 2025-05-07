
//Variables

//    .selector('node')              // Selects all nodes
//    .selector('edge')              // Selects all edges
//    .selector('#myNodeId')         // Selects a node or edge with ID "myNodeId"
//    .selector('.primaryNode')      // Selects all elements with class "primaryNode"
//    .selector('node.primaryNode')  // All nodes with class "primaryNode"
//    .selector('edge.highlighted')  // All edges with class "highlighted"

//====================
// VARIABLES & INITIALIZATION
//====================

var icons = [];
var dotNetHelper = {};
var selectedObject = {};
var isDragging = false;
var selectedNode = null;
var elements = [];
var metadataObj = {};

// Zoom and layout configuration
var zoomOptions = {
    level: 0.25
};

var layoutOptions = {
    name: 'preset',
    idealEdgeLength: 100,
    nodeOverlap: 20,
    refresh: 20,
    fit: true,
    padding: 30,
    randomize: false,
    componentSpacing: 100,
    nodeRepulsion: 400000,
    edgeElasticity: 100,
    nestingFactor: 5,
    gravity: 80,
    numIter: 1000,
    initialTemp: 200,
    coolingFactor: 0.95,
    minTemp: 1.0
};

// Grid options configuration
var grid_options = {
    // On/Off Modules
    /* From the following four snap options, at most one should be true at a given time */
    snapToGridOnRelease: true, // Snap to grid on release
    snapToGridDuringDrag: false, // Snap to grid during drag
    snapToAlignmentLocationOnRelease: false, // Snap to alignment location on release
    snapToAlignmentLocationDuringDrag: false, // Snap to alignment location during drag
    distributionGuidelines: false, // Distribution guidelines
    geometricGuideline: false, // Geometric guidelines
    initPosAlignment: false, // Guideline to initial mouse position
    centerToEdgeAlignment: false, // Center to edge alignment
    resize: false, // Adjust node sizes to cell sizes
    parentPadding: false, // Adjust parent sizes to cell sizes by padding
    drawGrid: true, // Draw grid background

    // General
    gridSpacing: 20, // Distance between the lines of the grid.
    snapToGridCenter: true, // Snaps nodes to center of gridlines. When false, snaps to gridlines themselves. Note that either snapToGridOnRelease or snapToGridDuringDrag must be true.

    // Draw Grid
    zoomDash: true, // Determines whether the size of the dashes should change when the drawing is zoomed in and out if grid is drawn.
    panGrid: false, // Determines whether the grid should move then the user moves the graph if grid is drawn.
    gridStackOrder: -1, // Namely z-index
    gridColor: '#007aff', // Color of grid lines
    lineWidth: 1.0, // Width of grid lines

    // Guidelines
    guidelinesStackOrder: 4, // z-index of guidelines
    guidelinesTolerance: 2.00, // Tolerance distance for rendered positions of nodes' interaction.
    guidelinesStyle: { // Set ctx properties of line. Properties are here:
        strokeStyle: "#8b7d6b", // color of geometric guidelines
        geometricGuidelineRange: 400, // range of geometric guidelines
        range: 100, // max range of distribution guidelines
        minDistRange: 10, // min range for distribution guidelines
        distGuidelineOffset: 10, // shift amount of distribution guidelines
        horizontalDistColor: "#ff0000", // color of horizontal distribution alignment
        verticalDistColor: "#00ff00", // color of vertical distribution alignment
        initPosAlignmentColor: "#0000ff", // color of alignment to initial mouse location
        lineDash: [0, 0], // line style of geometric guidelines
        horizontalDistLine: [0, 0], // line style of horizontal distribution guidelines
        verticalDistLine: [0, 0], // line style of vertical distribution guidelines
        initPosAlignmentLine: [0, 0], // line style of alignment to initial mouse position
    },

    // Parent Padding
    parentSpacing: -1 // -1 to set paddings of parents to gridSpacing
};

//====================
// CYTOSCAPE INITIALIZATION
//====================

// Initialize cytoscape
var cy = cytoscape({
    container: document.getElementById('cy'),
    style: [
        {
            selector: '.groupNode',
            style: {
                'text-margin-y': '5px',
                'text-valign': 'bottom',
                'shape': 'round-rectangle',
                'border-width': 1,
                'border-color': 'rgba(0, 122, 255, 1.0)',
                'content': 'data(name)',
                'color': 'white',
                'text-outline-width': 1,
                'text-outline-color': 'black',
                'background-color': 'grey'
            }
        },
        {
            selector: '.primaryNode',
            style: {
                'text-margin-y': '5px',
                'text-valign': 'bottom',
                'shape': 'round-rectangle',
                'border-width': 1,
                'border-color': 'rgba(0, 122, 255, 1.0)',
                'content': 'data(name)',
                'color': 'white',
                'text-outline-width': 1,
                'text-outline-color': 'black',
                'background-color': 'white',
                'background-image': function (ele) {
                    return makeSvg(ele).svg;
                },
                'width': 50,
                'height': 50
            }
        },
        {
            selector: 'edge',
            style: {
                'width': 'data(width)',
                'curve-style': 'data(curveStyle)',
                'line-color': 'data(lineColor)',
                'line-style': 'data(lineStyle)',
                'width': 'data(width)'
            }
        },
        {
            selector: 'node',
            style: {
                'background-color': 'data(backgroundColor)',
                'border-color': 'data(borderColor)',
            }
        },
        {
            selector: ':selected',
            style: {
                'background-color': 'rgba(0,122,255,1.0)',
                'source-arrow-color': 'rgba(0,122,255,1.0)',
                'text-outline-color': 'rgba(0,122,255,1.0)'
            }
        },
        {
            selector: ':parent',
            style: {
                'text-valign': 'top',
                'text-halign': 'center',
                'shape': 'round-rectangle',
                'corner-radius': 5,
                'padding': 10
            }
        },
        {
            selector: '.arrowTarget',
            style: {
                'target-arrow-shape': 'triangle'

            }
        },
        {
            selector: '.arrowBoth',
            style: {
                'source-arrow-shape': 'triangle',
                'target-arrow-shape': 'triangle'

            }
        }

    ],
    wheelSensitivity: 0.15,
    minZoom: 0.5,
    maxZoom: 2
});


//====================
// UTILITY FUNCTIONS
//====================

// Get icon by ID
function iconById(id) {
    var icon = icons.filter(function (icon) {
        return icon.id == id;
    });
    var results = icon[0].content;
    return results;
}

// Create SVG for node
var makeSvg = function (ele) {
    let eleName = ele.data('name');
    let eleIconId = ele.data('iconId');
    let svgString = "";

    if (!eleIconId) {
        // console.log("using default icon..");
        svgString = iconById("10040");
    }
    else {
        //  console.log("attempting to fetch icon");
        svgString = iconById(eleIconId);
    }

    let newWidth = "40";
    let newHeight = "40";
    let svgTagRegex = /<svg[^>]+>/;
    svgString = svgString.replace(svgTagRegex, (svgTag) => {
        return svgTag.replace(/width="\d+"/, `width="${newWidth}"`)
            .replace(/height="\d+"/, `height="${newHeight}"`);
    });
    //console.log(svgString);
    let encodedSvg = encodeURIComponent(svgString);
    let dataUrl = `data:image/svg+xml;charset=utf-8,${encodedSvg}`;

    return { svg: dataUrl };
};

// Set .NET object reference
function SetDotNetObjectRef(objref) {
    window.dotNetHelper = objref;
    console.log(window.dotNetHelper);
}

// Set icon collection
function SetIconCollection(objref) {
    icons = objref;
    console.log(icons);
}

// Change layout
function changeLayout(layout) {
    layoutOptions = layout;
}

//====================
// NODE AND EDGE OPERATIONS
//====================

// Add node
function addNode(target) {
    var mouseX = target.position.x;
    var mouseY = target.position.y;
    console.log("Executing Add Node");
    var results = window.dotNetHelper.invokeMethodAsync('AddServiceNode', mouseX, mouseY);
    results.then(function (r) {
        console.log(r);
    });
}

// Create edge between nodes
function createEdge(sourceNode, targetNode) {
    var results = window.dotNetHelper.invokeMethodAsync('AddEdge', sourceNode, targetNode);
    results.then(function (r) {
        console.log(r);
    });
}

// Connect selected nodes
function connectSelectedNodes() {
    var selectedNodes = cy.$('node:selected');

    // Ensure exactly two nodes are selected
    if (selectedNodes.length === 2) {
        var sourceNode = selectedNodes[0];
        var targetNode = selectedNodes[1];

        var sourceId = sourceNode.data('id');
        var targetId = targetNode.data('id');

        console.log(`Creating edge from ${sourceId} to ${targetId}`);

        // Connect the nodes using the existing createEdge function
        createEdge(sourceId, targetId);

        console.log(`Edge created between ${sourceId} and ${targetId}`);

        return true;
    } else {
        console.log("Exactly two nodes must be selected to create a connection");
        return false;
    }
}

// Create group from selected nodes
function createGroup(target) {
    var selectedNodes = cy.$('node:selected');

    // Ensure more than one node is selected
    if (selectedNodes.length > 1) {
        var sourceNode = selectedNodes[0];
        var targetNode = selectedNodes[1];

        var collection = Array(sourceNode.data('id'), targetNode.data('id'));

        console.log("Executing Group Node");

        var results = window.dotNetHelper.invokeMethodAsync('AddGroupNode', collection, 0, 0);

        results.then(function (r) {
            console.log(r);
        });
    }
}

// View metadata
function viewMetaData(elementId, elementType) {
    var results = window.dotNetHelper.invokeMethodAsync('ViewMetaData', elementId, elementType);
    results.then(function (r) {
        console.log(r);
    });
}

//====================
// COMMAND EXECUTION
//====================

// Execute command
function executeCommand(ele, command) {
    var id = ele.data('id');
    var serviceId = ele.data('serviceId');
    var projectId = ele.data('projectId');
    var type = ele.data('type');
    var name = ele.data('name');
    console.log(type);
    selectedObject = { id: id, projectId: projectId, serviceId: serviceId, type: type, name: name };

    if (type == 'node') {
        if (command == 'edit') {
            //this is calling a .net function and returning the results.
            var results = window.dotNetHelper.invokeMethodAsync('GetServiceNodeById', selectedObject);
            results.then(function (r) {
                console.log(r);
                var myModal = new bootstrap.Modal(document.getElementById('editNodeModal'));
                myModal.show(r);
            });
        }
        else if (command == 'selecticon') {
            //this is calling a .net function and returning the results.
            var results = window.dotNetHelper.invokeMethodAsync('GetServiceNodeById', selectedObject);
            results.then(function (r) {
                console.log(r);
                var myModal = new bootstrap.Modal(document.getElementById('iconPickerModal'));
                myModal.show(r);
            });
        }
        else if (command == 'remove') {
            var results = window.dotNetHelper.invokeMethodAsync('GetServiceNodeById', selectedObject);
            results.then(function (r) {
                if (confirm("This will remove all metadata, edges and the assiciated node. Are you sure you wish to continue?")) {
                    var results = window.dotNetHelper.invokeMethodAsync('RemoveServiceNode', selectedObject);
                    results.then(function (r) {
                        console.log(r);
                    });
                }
            });
        }
        else if (command == 'connect') {
            connectSelectedNodes();
        }
        else if (command == 'group') {
            createGroup(ele);
        }
    }

    if (type == 'edge') {
        if (command == 'edit') {
            var results = window.dotNetHelper.invokeMethodAsync('GetEdgeById', selectedObject);
            results.then(function (r) {
                console.log(r);
                var myModal = new bootstrap.Modal(document.getElementById('editEdgeModal'));
                myModal.show(r);
            });
        }
        else if (command == 'remove') {
            var results = window.dotNetHelper.invokeMethodAsync('GetEdgeById', selectedObject);
            results.then(function (r) {
                if (confirm("This will remove all metadata and the assiciated connection between these two nodes. Are you sure you wish to continue?")) {
                    var results = window.dotNetHelper.invokeMethodAsync('RemoveEdge', selectedObject);
                    results.then(function (r) {
                        console.log(r);
                    });
                }
            });
        }
    }
}

//====================
// MENU CREATION
//====================

// Create commands for node/edge context menu
function createCommands(ele) {
    var selectedNodesCount = cy.$('node:selected').length;

    var commands = [{
        content: 'Edit',
        select: function (ele) {
            executeCommand(ele, 'edit');
        }
    },
    {
        content: 'Connect',
        select: function (ele) {
            executeCommand(ele, 'connect');
        },
        enabled: (ele.data('type') == 'node' && selectedNodesCount === 2)
    },
    {
        content: 'Add To Group',
        select: function (ele) {
            executeCommand(ele, 'group');
        },
        enabled: (ele.data('type') == 'node' && selectedNodesCount === 2)
    },
    {
        content: 'Select icon',
        select: function (ele) {
            executeCommand(ele, 'selecticon');
        }

    },
    {
        content: 'Remove',
        select: function (ele) {
            executeCommand(ele, 'remove');
        }
    }];

    return commands;
}

// Create commands for core (canvas) context menu
function createCoreCommands(ele) {
    var selectedNodesCount = cy.$('node:selected').length;

    var commands = [{
        content: 'Add Node',
        select: function (ele, target) {
            addNode(target);
        }
    },
    {
        content: 'Add To Group',
        select: function (ele, target) {
            executeCommand(target, 'group');
        },
        enabled: (ele.data('type') == 'Node' && selectedNodesCount === 2)
    },
    {
        content: 'Connect',
        select: function (ele) {
            executeCommand(ele, 'connect');
        },
        enabled: (ele.data('type') == 'Node')
    }];

    return commands;
}

// Initialize context menu
function contextMenu(cy) {
    var defaultsctx = {
        menuRadius: function (ele) { return 100; }, // the outer radius (node center to the end of the menu) in pixels. It is added to the rendered size of the node. Can either be a number or function as in the example.
        selector: 'node , edge', // elements matching this Cytoscape.js selector will trigger cxtmenus
        commands: createCommands,
        fillColor: 'rgba(0, 0, 0, 0.75)', // the background colour of the menu
        activeFillColor: 'rgba(1, 105, 217, 0.75)', // the colour used to indicate the selected command
        activePadding: 20, // additional size in pixels for the active command
        indicatorSize: 24, // the size in pixels of the pointer to the active command, will default to the node size if the node size is smaller than the indicator size,
        separatorWidth: 3, // the empty spacing in pixels between successive commands
        spotlightPadding: 4, // extra spacing in pixels between the element and the spotlight
        adaptativeNodeSpotlightRadius: false, // specify whether the spotlight radius should adapt to the node size
        minSpotlightRadius: 24, // the minimum radius in pixels of the spotlight (ignored for the node if adaptativeNodeSpotlightRadius is enabled but still used for the edge & background)
        maxSpotlightRadius: 38, // the maximum radius in pixels of the spotlight (ignored for the node if adaptativeNodeSpotlightRadius is enabled but still used for the edge & background)
        openMenuEvents: 'cxttapstart', // space-separated cytoscape events that will open the menu; only `cxttapstart` and/or `taphold` work here
        itemColor: 'white', // the colour of text in the command's content
        itemTextShadowColor: 'transparent', // the text shadow colour of the command's content
        zIndex: 9999, // the z-index of the ui div
        atMouse: false, // draw menu at mouse position
        outsideMenuCancel: 10 // if set to a number, this will cancel the command if the pointer is released outside of the spotlight, padded by the number given
    };

    cy.cxtmenu(defaultsctx);

    var defaultsCorectx = {
        menuRadius: function (ele) { return 100; }, // the outer radius (node center to the end of the menu) in pixels. It is added to the rendered size of the node. Can either be a number or function as in the example.
        selector: 'core', // elements matching this Cytoscape.js selector will trigger cxtmenus
        commands: createCoreCommands,
        fillColor: 'rgba(0, 0, 0, 0.75)', // the background colour of the menu
        activeFillColor: 'rgba(1, 105, 217, 0.75)', // the colour used to indicate the selected command
        activePadding: 20, // additional size in pixels for the active command
        indicatorSize: 24, // the size in pixels of the pointer to the active command, will default to the node size if the node size is smaller than the indicator size,
        separatorWidth: 3, // the empty spacing in pixels between successive commands
        spotlightPadding: 4, // extra spacing in pixels between the element and the spotlight
        adaptativeNodeSpotlightRadius: false, // specify whether the spotlight radius should adapt to the node size
        minSpotlightRadius: 24, // the minimum radius in pixels of the spotlight (ignored for the node if adaptativeNodeSpotlightRadius is enabled but still used for the edge & background)
        maxSpotlightRadius: 38, // the maximum radius in pixels of the spotlight (ignored for the node if adaptativeNodeSpotlightRadius is enabled but still used for the edge & background)
        openMenuEvents: 'cxttapstart', // space-separated cytoscape events that will open the menu; only `cxttapstart` and/or `taphold` work here
        itemColor: 'white', // the colour of text in the command's content
        itemTextShadowColor: 'transparent', // the text shadow colour of the command's content
        zIndex: 9999, // the z-index of the ui div
        atMouse: false, // draw menu at mouse position
        outsideMenuCancel: 10 // if set to a number, this will cancel the command if the pointer is released outside of the spotlight, padded by the number given
    };

    cy.cxtmenu(defaultsCorectx);
}

//====================
// GRAPH MANAGEMENT
//====================

// Refresh the graph with new data


function refresh(data_collection) {
    //Please be advised, the json conversion between C# and Json seems to be causing camel casing for properties. so Node becomes node, NodeType becomes nodeType etc.etc..
    elements = [];

    for (var node of data_collection.nodes) {
        if (node.nodeType === "node") {
            // Regular node with icon
            elements.push({
                group: 'nodes',
                classes: 'primaryNode',
                data: {
                    projectId: node.projectId,
                    parent: node.parentId,
                    id: node.id,
                    serviceId: node.id,
                    label: node.name,
                    name: node.name,
                    iconId: node.iconId,
                    type: 'node',
                    //width: node.style.width,
                    //height: node.style.height,
                    //shape: node.style.shape,
                    //cornerRadius: node.style.cornerRadius,
                    backgroundColor: node.style.backgroundColor,
                    //backgroundOpacity: node.style.backgroundOpacity,
                    //backgroundFill: node.style.backgroundFill,
                    borderColor: node.style.borderColor
                    //borderWidth: node.style.borderWidth,
                    //borderStyle: node.style.borderStyle,
                    //borderOpacity: node.style.borderOpacity,
                    //padding: node.style.padding,
                    //color: node.style.color,
                    //fontSize: node.style.fontSize,
                    //fontFamily: node.style.fontFamily,
                    //textHAlign: node.style.textHAlign,
                    //textVAlign: node.style.textVAlign,
                    //zIndex: node.style.zIndex

                },
                position: { x: parseFloat(node.xpos), y: parseFloat(node.ypos) }
            });
        } else if (node.nodeType === "group") {
            // Group node without icon
            elements.push({
                group: 'nodes',
                classes: 'groupNode',
                data: {
                    projectId: node.projectId,
                    parent: node.parentId,
                    id: node.id,
                    serviceId: node.id,
                    label: node.name,
                    name: node.name,
                    type: 'node',
                    //width: node.style.width,
                    //height: node.style.height,
                    //shape: node.style.shape,
                    //cornerRadius: node.style.cornerRadius,
                    backgroundColor: node.style.backgroundColor,
                    //backgroundOpacity: node.style.backgroundOpacity,
                    //backgroundFill: node.style.backgroundFill,
                    borderColor: node.style.borderColor
                    //borderWidth: node.style.borderWidth,
                    //borderStyle: node.style.borderStyle,
                    //borderOpacity: node.style.borderOpacity,
                    //padding: node.style.padding,
                    //color: node.style.color,
                    //fontSize: node.style.fontSize,
                    //fontFamily: node.style.fontFamily,
                    //textHAlign: node.style.textHAlign,
                    //textVAlign: node.style.textVAlign,
                    //zIndex: node.style.zIndex
                },
                position: { x: parseFloat(node.xpos), y: parseFloat(node.ypos) }
            });
        }
    }

    for (var path of data_collection.edges) {
        //console.log(path.source)
        elements.push({
            group: 'edges',
            classes: 'arrowTarget',
            data: {
                projectId: node.projectId,
                id: path.id,
                name: path.source,
                source: path.source,
                target: path.destination,
                type: 'edge',
                width: path.style.width,
                curveStyle: path.style.curveStyle,
                lineColor: path.style.lineColor,
                lineStyle: path.style.lineStyle,
                //lineCap: path.style.lineCap,
                //lineDashPattern: path.style.lineDashPattern,
                //sourceArrowShape: path.style.sourceArrowShape,
                //targetArrowShape: path.style.targetArrowShape,
                //sourceArrowColor: path.style.sourceArrowColor,
                //targetArrowColor: path.style.targetArrowColor,
                //sourceArrowWidth: path.style.sourceArrowWidth,
                //targetArrowWidth: path.style.targetArrowWidth,
                //arrowScale: path.style.arrowScale,
                //lineOpacity: path.style.lineOpacity,
                //zIndex: path.style.zIndex
            }
        });
    }

    cy.elements().remove(); // Remove old elements

    cy.add(elements); // Add new elements

    let layout = cy.layout(layoutOptions);

    cy.zoom(zoomOptions);

    cy.fit();
    //https://github.com/iVis-at-Bilkent/cytoscape.js-grid-guide/blob/master/demo.html



    cy.gridGuide(grid_options);


    layout.run();
}

function ChangeOption(option, value) {
    if (option == "Grid")
        grid_options.drawGrid = !grid_options.drawGrid;
}
//====================
// EVENT LISTENERS
//====================

// Edge click event
cy.on('click', 'edge', function (evt) {
    var node = evt.target;
    var id = node.data('id');
    viewMetaData(id, 'edge');
});

// Node click event
cy.on('click', 'node', function (evt) {
    var node = evt.target;
    var id = node.data('id');
    console.log("Node clicked: ", id); // Log the ID or other identifying info of the clicked node
    viewMetaData(id, 'node');
});

// Node mousedown event
cy.on('mousedown', 'node', function (event) {
    // Start dragging when a node is clicked
    if (selectedNode == null) {
        console.log("mouse down event targetid :" + event.target.id())
        isDragging = true;
        selectedNode = event.target;
        console.log(selectedNode);
    }
});

// Mouse up event
cy.on('mouseup', function (event) {
    if (isDragging == true && selectedNode != null) {
        var pos = event.target.position();
        var target = event.target.data();
        console.log(target);

        var results = window.dotNetHelper.invokeMethodAsync('UpdateServiceNodePosition', target, pos.x, pos.y);
        results.then(function (r) {
            console.log(r);
        });

        // Stop dragging when the mouse button is released
        isDragging = false;
        selectedNode = null;
    }
});

//====================
// INITIALIZATION
//====================

// Initialize grid guide
cy.gridGuide(grid_options);

// Initialize context menu
contextMenu(cy);