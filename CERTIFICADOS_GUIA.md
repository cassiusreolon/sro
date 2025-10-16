# Configuração de Certificados - Guia Específico

## Você possui: .key, .p12 e .cer

### Opção 1 (Recomendada): Usar o arquivo .p12

O arquivo .p12 já contém tanto o certificado quanto a chave privada. Configure assim:

```json
{
  "ApiExterna": {
    "BaseUrl": "https://api-seguros-cert-insurconnect.b3.com.br",
    "RegistroEndpoint": "/registro",
    "TokenUrl": "https://api-seguros-cert-insurconnect.b3.com.br/oauth/token",
    "ClientId": "dd1717fb-ff04-417c-bd23-4412dd5cb166",
    "ClientSecret": "dbee8959-79f1-416f-8024-034d6768f8c2",
    "CertificadoPath": "C:\\caminho\\para\\seu\\certificado.p12",
    "CertificadoPassword": "3XDRZR",
    "CertificadoKeyPath": "",
    "CertificadoCerPath": "",
    "TimeoutSeconds": 30
  }
}
```

### Opção 2: Usar arquivos separados .key + .cer

Se preferir usar os arquivos separados:

```json
{
  "ApiExterna": {
    "BaseUrl": "https://api-seguros-cert-insurconnect.b3.com.br",
    "RegistroEndpoint": "/registro", 
    "TokenUrl": "https://api-seguros-cert-insurconnect.b3.com.br/oauth/token",
    "ClientId": "dd1717fb-ff04-417c-bd23-4412dd5cb166",
    "ClientSecret": "dbee8959-79f1-416f-8024-034d6768f8c2",
    "CertificadoPath": "",
    "CertificadoPassword": "3XDRZR",
    "CertificadoKeyPath": "C:\\caminho\\para\\sua\\chave.key",
    "CertificadoCerPath": "C:\\caminho\\para\\seu\\certificado.cer",
    "TimeoutSeconds": 30
  }
}
```

## Estrutura de Diretórios Sugerida

```
C:\certificados\
├── certificado.p12    ← Use este (mais simples)
├── chave.key         ← Ou estes dois em conjunto
└── certificado.cer   ← 
```

## Como Testar

1. **Coloque os arquivos no diretório**:
   ```
   C:\certificados\certificado.p12
   ```

2. **Atualize o appsettings.json**:
   ```json
   "CertificadoPath": "C:\\certificados\\certificado.p12"
   ```

3. **Execute o endpoint**:
   ```
   POST /api/documento/enviar
   ```

4. **Verifique os logs** para confirmar:
   - ✅ "Certificado cliente carregado com sucesso"
   - ✅ "Token obtido com sucesso"
   - ✅ "Documentos enviados com sucesso"

## Troubleshooting

### Erro: "Certificado não encontrado"
- Verifique se o caminho está correto
- Use barras duplas `\\` no Windows
- Confirme se o arquivo existe

### Erro: "Senha incorreta"
- Verifique se a senha "3XDRZR" está correta
- Teste se o arquivo .p12 abre com essa senha

### Erro: "Chave privada não encontrada"
- Se usar .key + .cer, verifique se ambos os arquivos existem
- Confirme se o arquivo .key está no formato correto (PEM)

### Formatos de Arquivo .key Suportados
O arquivo .key deve estar no formato PEM, algo como:
```
-----BEGIN PRIVATE KEY-----
MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQC7...
-----END PRIVATE KEY-----
```

Ou:
```
-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAu4f3bAkL9gJ8kN2dF7...
-----END RSA PRIVATE KEY-----
```