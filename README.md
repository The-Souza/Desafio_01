# Gerador de Arquivo JSON

<p align="center">
  <img alt="Static Badge" src="https://img.shields.io/badge/LICENSE-MIT-blue">
  <img alt="Static Badge" src="https://img.shields.io/badge/STATUS-EM_DESENVOLVIMENTO-green">
  <img alt="Language" src="https://img.shields.io/badge/LINGUAGEM-C%23-blueviolet">
</p>

## Descrição do Projeto

1. **Objetivo**

   Desenvolvido como uma aplicação de console em C# (.NET 8), este projeto gera um arquivo no formato JSON contendo uma coleção de objetos com exatamente quatro propriedades (A, B, C e D). Cada propriedade é preenchida com uma string alfanumérica aleatória de 6 caracteres.

2. **Especificações**

   * **2.1. Aplicação Console**

     * Requer dois parâmetros obrigatórios:

       * `--output`: Caminho de saída para o arquivo gerado.
       * `--size`: Tamanho alvo do arquivo, em megabytes (MB).

   * **2.2. Formato do JSON**

     * O arquivo final é composto por objetos com a estrutura:

       ```json
       { "A": "...", "B": "...", "C": "...", "D": "..." }
       ```

   * **2.3. Dados Aleatórios**

     * Cada campo (A, B, C, D) contém uma string alfanumérica com 6 caracteres gerada aleatoriamente.

   * **2.4. Tamanho do Arquivo**

     * Gera um arquivo com o tamanho aproximado desejado (tolerância de ±1%).
     * O limite máximo é de 400MB.
     * A quantidade de registros é ajustada dinamicamente durante a geração.

   * **2.5. Performance & Escalabilidade**

     * Não carrega toda a coleção em memória.
     * Utiliza streaming para escrita no arquivo com `System.Text.Json`.
     * O código informa como é feita a estimativa de registros e controle do loop.

## Funcionalidades do Projeto

* **Argumentos de Linha de Comando**: Recebe `--output` e `--size`. Verifica a existência do caminho de saída e imprime mensagens apropriadas. Converte e imprime o tamanho desejado em MB ou GB.

* **Tempo Gasto**: Utiliza um cronômetro para calcular e exibir o tempo total gasto na geração do arquivo JSON.

* **Gerador de Arquivo JSON**:

  * Geração de registros com strings aleatórias;
  * Exibição de barra de progresso;
  * Cálculo e exibição do tamanho do arquivo durante a geração;
  * Interrupção da geração ao atingir o tamanho alvo com tolerância.

* **Gerador de String Alfanumérica**:

  * Um método privado sorteia caracteres de uma string base alfanumérica;
  * Um segundo método expõe essa funcionalidade publicamente.

## Exemplo de Execução

```bash
dotnet run --output="C:\Users\guilherme2000925\Desktop\PastaDestino" --size=400
```

## Rodando o Código

<p align="center">
  <img src="img/img1.png" width="800" height="450" alt="imagem1">
  <img src="img/img2.png" width="800" height="450" alt="imagem2">
  <img src="img/img3.png" width="800" height="450" alt="imagem3">
  <img src="img/img4.png" width="800" height="450" alt="imagem4">
</p>

## Pré-requisitos

* .NET 8 SDK instalado
* Visual Studio ou qualquer editor C#
* Sistema operacional Windows recomendado (por enquanto)

## Técnicas e Tecnologias Utilizadas

* `System.Diagnostics`
* `System.Text.Json`
* `System.Text`