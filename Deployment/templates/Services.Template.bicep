@description('Name prefix for all resources')
param namePrefix string = 'servicegraph'

@description('Location for all resources')
param location string = 'canadacentral'

@description('Cosmos DB account name (must be globally unique)')
param cosmosDbAccountName string = '${namePrefix}cosmos'

@description('Cosmos DB database name')
param cosmosDbDatabaseName string = 'appdb'

@description('Cosmos DB container name')
param cosmosDbContainerName string = 'servicegraph'

@description('App Service plan SKU (e.g., "S1")')
param appServicePlanSku string = 'S1'

@description('Web Package for deployment')
param webBinaries string

@description('App Insights Name')
param appInsightsName string = 'cjdSvgAppInsightsIn'

@description('App Insights Name')
param workspaceName string = 'cjdSvgAIWorkspsace'

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${namePrefix}-asp'
  location: location
  sku: {
    name: appServicePlanSku
    tier: 'Standard'
  }
  properties: {
    reserved: false
  }
}



// Resource: Log Analytics Workspace (for workspace-based Application Insights)
resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: workspaceName
  location: location
  properties: {
    retentionInDays: 30
    sku: {
      name: 'PerGB2018'
    }
  }
}

// Resource: Application Insights
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalytics.id
  }
}

resource webAppService 'Microsoft.Web/sites@2022-03-01' = {
  name: '${namePrefix}-web'
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'DB__ConnectionString'
          value: cosmosDbAccount.listConnectionStrings().connectionStrings[0].connectionString
        }
        {
          name:'AzureAd__Domain'
          value:'microsoft.onmicrosoft.com'  
        }
        {
          name:'AzureAd__TenantId'
          value:'72f988bf-86f1-41af-91ab-2d7cd011db47'  
        }
        {
          name:'AzureAd__ClientId'
          value:'e0d5bf42-ea8e-4a0e-9217-5b0ac0cb29eb'  
        }
        {
          name:'AzureAd__CallbackPath'
          value:'/.auth/login/aad/callback'  
        }
        {
          name:'AzureAd__Scopes'
          value:'access_as_user'  
        }
        {
          name:'OVERRIDE_USE_MI_FIC_ASSERTION_CLIENTID'
          value:'db251f96-7897-41c0-b1af-0c4dbee6bdf2'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~3'
        }
      ]
    }
  }
  dependsOn: [
    appServicePlan
  ]
}

resource cosmosDbAccount 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' = {
  name: cosmosDbAccountName
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
      }
    ]
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
  }
}

resource cosmosDbDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-04-15' = {
  parent: cosmosDbAccount
  name: cosmosDbDatabaseName
  properties: {
    resource: {
      id: cosmosDbDatabaseName
    }
  }
  dependsOn: [
    cosmosDbAccount
  ]
}

resource cosmosDbContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  parent: cosmosDbDatabase
  name: cosmosDbContainerName
  properties: {
    resource: {
      id: cosmosDbContainerName
      partitionKey: {
        paths: ['/id']
        kind: 'Hash'
      }
    }
  }
  dependsOn: [
    cosmosDbDatabase
  ]
}

@description('MSDeploy packaged application to App Service Staging Slot')
resource webDeploy 'Microsoft.Web/sites/extensions@2023-12-01' = {
  name: any('ZipDeploy') // https://github.com/Azure/bicep/issues/9024
  parent: webAppService
  properties: {
    packageUri: webBinaries
    appOffline: true
  }
}

output webAppUrl string = 'https://${webAppService.properties.defaultHostName}'
output cosmosDbEndpoint string = cosmosDbAccount.properties.documentEndpoint
