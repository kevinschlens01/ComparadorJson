using System;
using System.Collections.Generic;
using System.Text.Json;

public class JsonEvaluator
{
    public static (List<string> MissingFields, List<string> ExtraFields) EvaluateJson(string jsonTemplate, string jsonToEvaluate)
    {
        var missingFields = new List<string>();
        var extraFields = new List<string>();

        using (JsonDocument templateDoc = JsonDocument.Parse(jsonTemplate))
        using (JsonDocument evaluateDoc = JsonDocument.Parse(jsonToEvaluate))
        {
            CheckFields(templateDoc.RootElement, evaluateDoc.RootElement, "", missingFields, extraFields);
        }

        return (missingFields, extraFields);
    }

    private static void CheckFields(JsonElement templateElement, JsonElement evaluateElement, string currentPath, List<string> missingFields, List<string> extraFields)
    {
        var templateFields = new HashSet<string>();

        // Verifica os campos que estão presentes no template
        foreach (JsonProperty templateProperty in templateElement.EnumerateObject())
        {
            string fullPath = string.IsNullOrEmpty(currentPath) ? templateProperty.Name : $"{currentPath}.{templateProperty.Name}";
            templateFields.Add(templateProperty.Name);

            if (evaluateElement.TryGetProperty(templateProperty.Name, out JsonElement evaluatedProperty))
            {
                // Verifica recursivamente objetos aninhados
                if (templateProperty.Value.ValueKind == JsonValueKind.Object)
                {
                    CheckFields(templateProperty.Value, evaluatedProperty, fullPath, missingFields, extraFields);
                }
            }
            else
            {
                missingFields.Add(fullPath);
            }
        }

        // Verifica os campos que estão presentes no JSON avaliado, mas não no template
        foreach (JsonProperty evaluatedProperty in evaluateElement.EnumerateObject())
        {
            string fullPath = string.IsNullOrEmpty(currentPath) ? evaluatedProperty.Name : $"{currentPath}.{evaluatedProperty.Name}";

            if (!templateFields.Contains(evaluatedProperty.Name))
            {
                extraFields.Add(fullPath);
            }
        }
    }

    public static void Main()
    {
        string jsonTemplate = @"{
          ""id"": 0,
    ""portal_Id"": 0,
    ""hash_Registro"": ""string"",
    ""hash_Cliente"": ""string"",
    ""seq_Gg_Empresa"": 0,
    ""nom_Empresa"": ""string"",
    ""nro_Empresa_Cnpj"": ""string"",
    ""seq_Gg_Filial"": 0,
    ""nom_Filial"": ""string"",
    ""nro_Filial_Cnpj"": ""string"",
    ""seq_Ct_Convenio"": 0,
    ""sbl_Convenio_Tipo"": ""string"",
    ""nom_Convenio_Tipo"": ""string"",
    ""sbl_Convenio_Subtipo"": ""string"",
    ""nom_Convenio_Subtipo"": ""string"",
    ""ano_Convenio"": 0,
    ""nrd_Convenio"": ""string"",
    ""nom_Convenio"": ""string"",
    ""dat_Convenio"": ""2024-08-21T16:03:20.311Z"",
    ""dat_Convenio_Inicio"": ""2024-08-21T16:03:20.311Z"",
    ""dat_Convenio_Fim"": ""2024-08-21T16:03:20.311Z"",
    ""mes_Convenio"": ""string"",
    ""ano_Processo"": 0,
    ""nrd_Processo"": ""string"",
    ""seq_CtConvenio_Participante"": 0,
    ""sbl_Pessoa_Participante_Tipo"": ""string"",
    ""nom_Pessoa_Participante_Tipo"": ""string"",
    ""sbl_Convenio_Legislacao"": ""string"",
    ""nom_Convenio_Legislacao"": ""string"",
    ""nro_Convenio_Legislacao"": ""string"",
    ""seq_Gg_Pessoa"": 0,
    ""nom_Pessoa"": ""string"",
    ""nro_Pessoa_Cpf_Cnpj"": ""string"",
    ""hst_Convenio"": ""strin"",
    ""vlr_Convenio"": 0,
    ""vlr_Convenio_Contrapartida"": 0,
    ""anexo"": ""s"",
    ""hash_Registro_Cpe"": ""string"",
    ""controle_Cpe"": ""string""
        }";

        string jsonToEvaluate = @"{
          ""id"": 0,
    ""portal_Id"": 0,
    ""hash_Registro"": ""string"",
    ""hash_Cliente"": ""string"",
    ""seq_Gg_Empresa"": 0,
    ""nom_Empresa"": ""string"",
    ""nro_Empresa_Cnpj"": ""string"",
    ""seq_Gg_Filial"": 0,
    ""nom_Filial"": ""string"",
    ""nro_Filial_Cnpj"": ""string"",
    ""ano_Convenio"": 0,
    ""nrd_Convenio"": ""string"",
    ""nom_Convenio"": ""string"",
    ""dat_Convenio"": ""2024-08-21T16:17:39.324Z"",
    ""mes_Convenio"": ""string"",
    ""ano_Processo"": 0,
    ""nrd_Processo"": ""string"",
    ""seq_CtConvenio_Participante"": 0,
    ""sbl_Pessoa_Participante_Tipo"": ""string"",
    ""nom_Pessoa_Participante_Tipo"": ""string"",
    ""nro_Pessoa_Cpf_Cnpj"": ""string"",
    ""hst_Convenio"": ""string"",
    ""vlr_Convenio"": 0,
    ""anexo"": ""string"",
    ""hash_Registro_Cpe"": ""string"",
    ""controle_Cpe"": ""string"",
    ""sbl_Convenio_Tipo"": ""string"",
    ""seq_Ct_Convenio"": 0,
    ""nom_Convenio_Tipo"": ""string"",
    ""sbl_Convenio_Subtipo"": ""string"",
    ""nom_Convenio_Subtipo"": ""string"",
    ""dat_Convenio_Inicio"": ""2024-08-21T16:17:39.324Z"",
    ""dat_Convenio_Fim"": ""2024-08-21T16:17:39.324Z"",
    ""sbl_Convenio_Legislacao"": ""string"",
    ""nom_Convenio_Legislacao"": ""string"",
    ""nro_Convenio_Legislacao"": ""string"",
    ""seq_Gg_Pessoa"": 0,
    ""nom_Pessoa"": ""string"",
    ""vlr_Convenio_Contrapartida"": 0
        }";

        var (missingFields, extraFields) = EvaluateJson(jsonTemplate, jsonToEvaluate);

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

        if (extraFields.Count > 0)
        {
            Console.WriteLine("\nOs seguintes campos estão presentes no JSON avaliado, mas não no JSON padrão:");
            foreach (string field in extraFields)
            {
                Console.WriteLine(field);
            }
        }
        else
        {
            Console.WriteLine("\nNão há campos extras no JSON avaliado.");
        }
    }
}
