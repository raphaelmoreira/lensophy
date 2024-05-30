# Atualizando a documentação
Após efetuar mudanças na biblioteca `Lensophy`, seja no código ou em sua documentação, certifique-se de efetuar uma `build`
em modo `release`. Isso garante que o arquivo `Lensophy.Doc.Lensophy.xml` seja atualizado e/ou gerado corretamente.

# Requisistos
Certifique-se de que o DocFx esteja instalado em seu ambiente como uma ferramenta global. Para isso, efetue o seguinte comando:

```batch
dotnet tool install docfx --global
```
Caso já possua a ferramenta, aproveite para atualizá-la:

```batch
dotnet tool update -g docfx
```
Em seguida, assumindo que você está na raiz do projeto, execute o seguinte comando:

```batch
docfx tool/docfx_project/docfx.json --serve
```
Isso irá compilar e publicar uma versão atualizada da documentação em `localhost:8080`, lhe permitindo verificar se tudo está 
conforme esperado. Após conferência, basta efetuar o **commit/push**, que o deploy se encarrega do resto.

>Nota: assim que possível, irei automatizar este processo.</i>

# Criando documentação para um novo projeto
Essa solução está configurada para gerar a documentação de todas as bibliotecas, contudo, optei por produzir apenas o conteúdo
da `Lensophy`, que é o pacote NuGet efetivamente. Se você precisa criar uma estrutura totalmente nova, siga os passos à seguir:

>Nota: garanta que você atende aos [Requisitos](#requisistos).

## Iniciando nova estrutura
A estrutura deve estar na raiz e se chamar "docfx_project". Neste projeto, optei por colocá-la numa pasta previamente criada,
chamada "tool". Sendo assim:

```batch
cd tool
docfx init
```
Em seguida, lhe será solicitado algumas informações:
- **Name**: o nome da pasta onde o conteúdo gerado será armazenado (ex: site)
- **Generate .NET API documentation**: geralmente, a opção é `y`, exceto se estiver apenas interessado em produzir artigos (sem código).
- **Markdown docs location**: caso esteja lidando com código, informe a localização do arquivo `xml` de documentação.
- **Enable site search**: se deseja a função de pesquisa em seu site.
- **Enable PDF**: permite a gerar uma versão PDF do seu conteúdo.

Esse conjunto de informações irá produzir o arquivo `tool/docfx.json`, o qual pode ser modificado a qualquer momento para atender
outros cenários e configurações.

A título de exemplo, para o `Lensophy`, a estrutura dedicada ao `csproj` ficou assim:

```json
{
  "metadata": [
    {
      "src": [
        {
          "files": [
            "Lensophy/Lensophy.csproj"
          ],
          "src": "../../src"
        }
      ],
      "output": "../../doc/site"
    }]
}
```
Se estiver lidando apenas com artigos, talvez você queira remover algumas coisas. No geral, a estrutura criada será parecida
com isso:

| Mode   | LastWriteTime        | Length | Name       |
|--------|----------------------|--------|------------|
| d----- | 30/05/2024     09:23 |        | docs       |
| -a---- | 30/05/2024     09:23 | 501    | docfx.json |                                                                                                                                                                         
| -a---- | 30/05/2024     09:23 | 261    | index.md   |
| -a---- | 30/05/2024     09:23 | 27     | toc.yml    |


Por fim, execute o comando abaixo para compilar e publicar:

```batch
docfx tool/docfx_project/docfx.json --serve
```