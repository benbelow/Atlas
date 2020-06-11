output "function_app" {
  value = {
    hostname = azurerm_function_app.atlas_match_prediction_function.default_hostname
  }
}

output "storage" {
  value = {
    haplotype_frequency_set_container_name = azurerm_storage_container.haplotype_frequency_set_blob_container.name
  }
}

output "sql_database" {
  value = {
    name = azurerm_sql_database.atlas-match-prediction.name
  }
}