resource "azurerm_sql_database" "atlas-donor-import" {
  name = lower("${local.environment}-ATLAS-DONORS")
  resource_group_name = azurerm_resource_group.atlas_resource_group.name
  location = local.location
  server_name = azurerm_sql_server.atlas_sql_server.name
  tags = local.common_tags
}