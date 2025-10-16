# Configuração da API Externa B3

Este documento descreve como configurar a integração com a API Externa B3 para envio de documentos SRO.

## Configuração dos Certificados

### 1. Preparação do Certificado

Certifique-se de que você possui:
- Certificado digital em um dos formatos:
  - **Arquivo único**: .pfx ou .p12 (recomendado)
  - **Arquivos separados**: .key (chave privada) + .cer (certificado público)
- Senha do certificado (se aplicável)
- Client ID e Client Secret fornecidos pela B3

#### Formatos Suportados:
- **P12/PFX**: Arquivo único contendo certificado e chave privada
- **KEY + CER**: Chave privada (.key) e certificado público (.cer) em arquivos separados

### 2. Configuração dos Arquivos appsettings

#### appsettings.json (Produção)

**Opção 1 - Arquivo P12/PFX:**
```json
{
  "ApiExterna": {
    "BaseUrl": "https://api-seguros-cert-insurconnect.b3.com.br",
    "RegistroEndpoint": "/registro",
    "TokenUrl": "https://api-seguros-cert-insurconnect.b3.com.br/oauth/token",
    "ClientId": "SEU_CLIENT_ID_AQUI",
    "ClientSecret": "SEU_CLIENT_SECRET_AQUI",
    "CertificadoPath": "C:\\certificados\\producao\\certificado.p12",
    "CertificadoPassword": "senha_do_certificado",
    "CertificadoKeyPath": "",
    "CertificadoCerPath": "",
    "TimeoutSeconds": 30
  }
}
```

**Opção 2 - Arquivos Separados KEY + CER:**
```json
{
  "ApiExterna": {
    "BaseUrl": "https://api-seguros-cert-insurconnect.b3.com.br",
    "RegistroEndpoint": "/registro",
    "TokenUrl": "https://api-seguros-cert-insurconnect.b3.com.br/oauth/token",
    "ClientId": "SEU_CLIENT_ID_AQUI",
    "ClientSecret": "SEU_CLIENT_SECRET_AQUI",
    "CertificadoPath": "",
    "CertificadoPassword": "senha_se_necessaria",
    "CertificadoKeyPath": "C:\\certificados\\producao\\chave.key",
    "CertificadoCerPath": "C:\\certificados\\producao\\certificado.cer",
    "TimeoutSeconds": 30
  }
}
```

#### appsettings.Development.json (Desenvolvimento)
```json
{
  "ApiExterna": {
    "BaseUrl": "https://api-seguros-cert-insurconnect.b3.com.br",
    "RegistroEndpoint": "/registro",
    "TokenUrl": "https://api-seguros-cert-insurconnect.b3.com.br/oauth/token",
    "ClientId": "SEU_CLIENT_ID_DEV_AQUI",
    "ClientSecret": "SEU_CLIENT_SECRET_DEV_AQUI",
    "CertificadoPath": "C:\\certificados\\desenvolvimento\\certificado.pfx",
    "CertificadoPassword": "senha_do_certificado_dev",
    "TimeoutSeconds": 60
  }
}
```

### 3. Estrutura de Diretórios Recomendada

```
C:\certificados\
├── desenvolvimento\
│   └── certificado.pfx
└── producao\
    └── certificado.pfx
```

### 4. Permissões

Certifique-se de que:
- O arquivo de certificado tem as permissões corretas para ser lido pela aplicação
- A conta de serviço da aplicação tem acesso ao diretório dos certificados
- Em ambiente de produção, considere usar o Certificate Store do Windows

## Fluxo de Autenticação

1. **Client Credentials Flow**: A aplicação usa Client ID e Client Secret para obter um token OAuth2
2. **Certificado mTLS**: O certificado é usado para autenticação mútua TLS
3. **Cache de Token**: O token é armazenado em cache até próximo da expiração

## Endpoints Utilizados

- **Token**: `POST /oauth/token` - Obtenção do token de acesso
- **Registro**: `POST /registro` - Envio dos documentos

## Payload do Registro

```json
{
  "data": {
    "documento": [
      {
        // Estrutura do DocumentoRequestDto
      }
    ]
  }
}
```

## Respostas da API

### Sucesso (HTTP 200)
```json
{
  "data": {
    "identificador_lote": "uuid-do-lote"
  }
}
```

### Erro de Validação (HTTP 422)
```json
{
  "erros": [
    {
      "codigo": "ERRO_001",
      "titulo": "Campo obrigatório",
      "detalhe": "O campo 'codigoSeguradora' é obrigatório"
    }
  ]
}
```

## Logs

A aplicação registra logs detalhados:
- **Information**: Envios bem-sucedidos, obtenção de tokens
- **Warning**: Erros de validação da API externa
- **Error**: Erros de rede, certificados, timeouts
- **Debug**: Payloads JSON (apenas em desenvolvimento)

## Troubleshooting

### Erro de Certificado
- Verifique se o caminho do certificado está correto
- Confirme se a senha está correta
- Verifique se o certificado não expirou

### Erro de Autenticação
- Confirme Client ID e Client Secret
- Verifique se a URL do token está correta
- Confirme se o certificado é válido para o ambiente

### Timeout
- Aumente o valor de `TimeoutSeconds` se necessário
- Verifique conectividade de rede com a API B3

### Erro de Serialização
- Confirme se a estrutura dos DTOs está correta
- Verifique logs de Debug para visualizar o JSON enviado