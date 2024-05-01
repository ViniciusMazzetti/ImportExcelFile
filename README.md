# ImportExcelFile

# Sobre o projeto

Esta aplicação foi desenvolvida com finalidade de teste pratico. A ideia é criar uma forma de importar uma planilha com 1 milhão de registros e depois realizar consultas utilizando filtros.

A aplicação consiste no back-end do projeto, portanto, aqui estão criadas todas as regras de aplicação, tal como a importação dos dados e a forma como os filtros são realizados.

# Tecnologias utilizadas
## Back end
- .net 8
- Sql Server
- Dapper

# Features
- Importação de Dados de planilha excel;
- Consulta de dados no banco sql server;
- Criação automatica do banco e suas tabelas;

# Como executar o projeto

Pré-requisitos: Sql Server, .net 8

```bash
# clonar repositório
https://github.com/ViniciusMazzetti/ImportExcelFile.git

# abrir o projeto e compilar
Abrir a solution "ExcelFileImport.sln" no visual studio e compilar

# definir o projeto de inicialização
ExcelFileImport.API

# executar o projeto
```

# Autor

Vinícius Alexandre Mazzetti Rodel