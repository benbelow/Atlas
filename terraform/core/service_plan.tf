resource "azurerm_app_service_plan" "atlas" {
  name                = "${local.environment}-ATLAS"
  location            = local.location
  resource_group_name = azurerm_resource_group.atlas_resource_group.name

  sku {
    tier = var.SERVICE_PLAN_SKU["tier"]
    size = var.SERVICE_PLAN_SKU["size"]
  }
}

resource "azurerm_app_service_plan" "atlas-consumption-plan" {
  name                = "${local.environment}-ATLAS-CONSUMPTION-PLAN"
  location            = local.location
  resource_group_name = azurerm_resource_group.atlas_resource_group.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_app_service_plan" "atlas-elastic-plan" {
  name                         = "${local.environment}-ATLAS-ELASTIC-PLAN"
  location                     = local.location
  resource_group_name          = azurerm_resource_group.atlas_resource_group.name
  kind                         = "elastic"
  maximum_elastic_worker_count = 50

  sku {
    tier = "ElasticPremium"
    size = "EP1"
  }
}