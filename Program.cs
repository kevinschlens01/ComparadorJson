using System;
using System.Collections.Generic;
using System.Text.Json;

public class JsonEvaluator
{
    public static List<string> EvaluateJson(string jsonTemplate, string jsonToEvaluate)
    {
        var missingFields = new List<string>();

        using (JsonDocument templateDoc = JsonDocument.Parse(jsonTemplate))
        using (JsonDocument evaluateDoc = JsonDocument.Parse(jsonToEvaluate))
        {
            CheckFields(templateDoc.RootElement, evaluateDoc.RootElement, "", missingFields);
        }

        return missingFields;
    }

    private static void CheckFields(JsonElement templateElement, JsonElement evaluateElement, string currentPath, List<string> missingFields)
    {
        foreach (JsonProperty templateProperty in templateElement.EnumerateObject())
        {
            string fullPath = string.IsNullOrEmpty(currentPath) ? templateProperty.Name : $"{currentPath}.{templateProperty.Name}";

            if (evaluateElement.TryGetProperty(templateProperty.Name, out JsonElement evaluatedProperty))
            {
                // Recursively check nested objects
                if (templateProperty.Value.ValueKind == JsonValueKind.Object)
                {
                    CheckFields(templateProperty.Value, evaluatedProperty, fullPath, missingFields);
                }
            }
            else
            {
                missingFields.Add(fullPath);
            }
        }
    }

    public static void Main()
    {
        string jsonTemplate = @"{
          ""portaL_ID"": 0,
    ""hasH_CLIENTE"": ""string"",
    ""boL_TRANSMITIR_REGISTRO"": ""s"",
    ""seQ_GG_EMPRESA"": 0,
    ""noM_EMPRESA"": ""string"",
    ""nrO_EMPRESA_CNPJ"": ""string"",
    ""seQ_GG_FILIAL"": 0,
    ""noM_FILIAL"": ""string"",
    ""nrO_FILIAL_CNPJ"": ""string"",
    ""sbL_DIVIDA_FUNDADA"": ""s"",
    ""noM_DIVIDA_FUNDADA_TIPO"": ""string"",
    ""seQ_CT_UNIDADE_GESTORA"": 0,
    ""seQ_CT_DIVIDA_FUNDADA"": 0,
    ""noM_DIVIDA_TIPO_NATUREZA"": ""string"",
    ""noM_DIVIDA_FUNDADA"": ""string"",
    ""nrO_DIVIDA_FUNDADA_LEI"": ""string"",
    ""anO_DIVIDA_FUNDADA_LEI"": ""stri"",
    ""daT_DIVIDA_FUNDADA_LEI"": ""2024-08-21T09:54:30.107Z"",
    ""noM_PESSOA"": ""string"",
    ""nrO_DIVIDA_FUNDADA_PROCESSO"": ""string"",
    ""anO_DIVIDA_FUNDADA_PROCESSO"": ""stri"",
    ""nrO_DIVIDA_FUNDADA_CONTRATO"": ""string"",
    ""daT_DIVIDA_FUNDADA_CONTRATO"": ""2024-08-21T09:54:30.107Z"",
    ""daT_DIVIDA_FUNDADA_PUBLICACAO"": ""2024-08-21T09:54:30.107Z"",
    ""anO_COMPETENCIA_DIVIDA_FUNDADA"": ""stri"",
    ""ndA_COMPETENCIA"": ""string"",
    ""stR_DIVIDA_FUNDADA_VEICULO_PUBLICACAO"": ""string"",
    ""vlR_DIVIDA_FUNDADA_MOEDA_CONTRATADA"": 0,
    ""vlR_DIVIDA_FUNDADA_MOEDA_CORRENTE"": 0,
    ""vlR_DIVIDA_FUNDADA_SALDO"": 0,
    ""hasH_REGISTRO_AREA"": ""string"",
    ""controlE_AREA"": ""string""
        }";

        string jsonToEvaluate = @"{
  
    ""portaL_ID"": 0,
    ""hasH_CLIENTE"": ""string"",
    ""boL_TRANSMITIR_REGISTRO"": ""string"",
    ""seQ_GG_EMPRESA"": 0,
    ""noM_EMPRESA"": ""string"",
    ""nrO_EMPRESA_CNPJ"": ""string"",
    ""seQ_GG_FILIAL"": 0,
    ""noM_FILIAL"": ""string"",
    ""nrO_FILIAL_CNPJ"": ""string"",
    ""sbL_DIVIDA_FUNDADA"": ""string"",
    ""noM_DIVIDA_FUNDADA_TIPO"": ""string"",
    ""seQ_CT_UNIDADE_GESTORA"": 0,
    ""seQ_CT_DIVIDA_FUNDADA"": 0,
    ""noM_DIVIDA_TIPO_NATUREZA"": ""string"",
    ""noM_DIVIDA_FUNDADA"": ""string"",
    ""nrO_DIVIDA_FUNDADA_LEI"": ""string"",
    ""anO_DIVIDA_FUNDADA_LEI"": ""string"",
    ""daT_DIVIDA_FUNDADA_LEI"": ""2024-08-21T09:54:44.300Z"",
    ""noM_PESSOA"": ""string"",
    ""nrO_DIVIDA_FUNDADA_PROCESSO"": ""string"",
    ""anO_DIVIDA_FUNDADA_PROCESSO"": ""string"",
    ""nrO_DIVIDA_FUNDADA_CONTRATO"": ""string"",
    ""daT_DIVIDA_FUNDADA_CONTRATO"": ""2024-08-21T09:54:44.300Z"",
    ""daT_DIVIDA_FUNDADA_PUBLICACAO"": ""2024-08-21T09:54:44.300Z"",
    ""anO_COMPETENCIA_DIVIDA_FUNDADA"": ""string"",
    ""ndA_COMPETENCIA"": ""string"",
    ""stR_DIVIDA_FUNDADA_VEICULO_PUBLICACAO"": ""string"",
    ""vlR_DIVIDA_FUNDADA_MOEDA_CONTRATADA"": 0,
    ""vlR_DIVIDA_FUNDADA_MOEDA_CORRENTE"": 0,
    ""vlR_DIVIDA_FUNDADA_SALDO"": 0,
    ""hasH_REGISTRO_AREA"": ""string"",
    ""controlE_AREA"": ""string""
  }";

        List<string> missingFields = EvaluateJson(jsonTemplate, jsonToEvaluate);

        if (missingFields.Count > 0)
        {
            Console.WriteLine("Os seguintes campos estão faltando no JSON avaliado:");
            foreach (string field in missingFields)
            {
                Console.WriteLine(field);
            }
        }
        else
        {
            Console.WriteLine("Todos os campos estão presentes.");
        }
    }
}
