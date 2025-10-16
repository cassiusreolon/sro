# 🔍 Diagnóstico do Erro "Forbidden" na API B3

## ❌ Problema Atual
```
System.Net.Http.HttpRequestException: Erro ao obter token: Forbidden
```

## 🔧 Passos para Diagnóstico

### 1. **Verificar Certificados**
Execute o endpoint de teste:
```
GET /api/TesteApiExterna/testar-certificado
```

Isso vai mostrar:
- ✅ Se os arquivos de certificado existem
- ✅ Quais arquivos estão configurados
- ✅ Status da configuração

### 2. **Testar Conectividade**
Execute o endpoint:
```
GET /api/TesteApiExterna/testar-conectividade
```

Verifica se consegue acessar:
- Base URL da B3
- URL do token OAuth

### 3. **Testar Obtenção de Token**
Execute o endpoint:
```
GET /api/TesteApiExterna/testar-token
```

Vai tentar obter o token e mostrar erro detalhado.

## 🔍 Possíveis Causas do Erro "Forbidden"

### 1. **Certificado Incorreto ou Inválido**
- ❓ Certificado expirado
- ❓ Certificado não é o correto para este ambiente
- ❓ Senha do certificado incorreta
- ❓ Formato do certificado incompatível

**Verificação**:
```bash
# Verificar validade do certificado .p12
openssl pkcs12 -in "C:\Certificados\3123-3126_CERT.p12" -info -noout -passin pass:3XDRZR
```

### 2. **Client Credentials Incorretos**
- ❓ Client ID incorreto: `dd1717fb-ff04-417c-bd23-4412dd5cb166`
- ❓ Client Secret incorreto: `dbee8959-79f1-416f-8024-034d6768f8c2`
- ❓ Credenciais para ambiente errado (dev vs prod)

### 3. **Ambiente Incorreto**
- ❓ Usando credenciais de produção em homologação
- ❓ URL incorreta para o ambiente
- ❓ Certificado de um ambiente sendo usado em outro

### 4. **Problema de Rede/Firewall**
- ❓ IP não autorizado na B3
- ❓ Proxy bloqueando certificados mTLS
- ❓ Firewall bloqueando porta 443

### 5. **Configuração mTLS**
- ❓ B3 requer certificado mTLS na requisição do token
- ❓ Certificado não está sendo enviado corretamente

## 🛠️ Soluções Propostas

### ✅ **Solução 1: Verificar Certificado**
1. Confirme que o arquivo existe:
   ```
   C:\Certificados\3123-3126_CERT.p12
   ```

2. Teste se a senha está correta:
   ```csharp
   try {
       var cert = new X509Certificate2("C:\\Certificados\\3123-3126_CERT.p12", "3XDRZR");
       Console.WriteLine($"Certificado válido: {cert.Subject}");
       Console.WriteLine($"Válido até: {cert.NotAfter}");
   } catch (Exception ex) {
       Console.WriteLine($"Erro: {ex.Message}");
   }
   ```

### ✅ **Solução 2: Testar com Postman/Curl**
Teste manualmente a autenticação:

```bash
curl -X POST "https://api-seguros-cert-insurconnect.b3.com.br/oauth/token" \
  --cert "C:\Certificados\3123-3126_CERT.p12:3XDRZR" \
  --data "grant_type=client_credentials&client_id=dd1717fb-ff04-417c-bd23-4412dd5cb166&client_secret=dbee8959-79f1-416f-8024-034d6768f8c2"
```

### ✅ **Solução 3: Validar com a B3**
Confirme com a B3:
- ✅ Client ID e Secret estão corretos
- ✅ Certificado está válido e associado às credenciais  
- ✅ IP está autorizado
- ✅ Ambiente está correto

### ✅ **Solução 4: Logs Detalhados**
O sistema agora vai mostrar logs detalhados:

```json
{
  "erro": "Erro ao obter token: Status: Forbidden, Resposta: {...}",
  "possiveisCausas": [
    "Client ID ou Client Secret incorretos",
    "Certificado não configurado ou inválido", 
    "Certificado expirado",
    "IP não autorizado na B3",
    "Ambiente incorreto (produção vs homologação)"
  ]
}
```

## 📋 **Checklist de Verificação**

- [ ] Arquivo de certificado existe no caminho especificado
- [ ] Senha do certificado está correta
- [ ] Certificado não está expirado
- [ ] Client ID e Client Secret estão corretos
- [ ] URL do token está correta para o ambiente
- [ ] IP está autorizado na B3
- [ ] Não há proxy interferindo
- [ ] Certificado foi emitido para este ambiente específico

## 🔄 **Próximos Passos**

1. Execute os endpoints de teste
2. Verifique os logs detalhados
3. Confirme dados com a B3
4. Teste manualmente com curl/Postman se necessário

## 📞 **Contato B3**
Se o problema persistir, entre em contato com a B3 com:
- Client ID usado
- Horário do erro
- IP de origem
- Certificado usado (subject/thumbprint)