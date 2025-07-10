# Gerador de Arquivo JSON

Projeto desenvolvido em C# (.NET 8) que gera um arquivo `.json` contendo uma lista de objetos com propriedades alfanuméricas aleatórias, com controle de tamanho de saída e uso eficiente de memória e CPU.

---

## 📂 Estrutura dos Arquivos

### 1. `Program.cs`

Responsável por:

* Capturar e validar os argumentos passados via linha de comando:

  * `--output=<diretório de saída>`
  * `--size=<tamanho em MB>`
* Exibir o diretório e o tamanho formatado no console;
* Iniciar o cronômetro e executar a geração do JSON através da classe `VerTempoGasto`.

### 2. `GeradorStringAlfanumerico.cs`

Classe que gera strings alfanuméricas aleatórias de 6 caracteres. Funcionalidades:

* Usa `StringBuilder` para eficiência;
* Contém um método público que retorna uma string com letras e números aleatórios;
* Utilizada repetidamente para preencher os campos A, B, C e D de cada objeto JSON.

### 3. `VerTempoGasto.cs`

Classe utilitária que mede o tempo total de execução do processo. Funcionalidades:

* Utiliza `System.Diagnostics.Stopwatch` para medir o tempo de execução;
* Exibe o tempo decorrido no console ao final do processo.

### 4. `GeradorArquivoJSON.cs`

A principal classe do projeto. Responsável por:

* Gerar uma lista de objetos com campos A, B, C e D preenchidos aleatoriamente;
* Dividir os dados em partes e salvar como arquivos temporários paralelamente (uso de `Parallel.For`);
* Combinar os arquivos temporários em um único JSON final, respeitando o limite de tamanho;
* Exibir barras de progresso e estatísticas no console;
* Validar o tamanho final do arquivo, com tolerância de ±1%;
* Apagar arquivos temporários ao final ou em caso de erro.

---

## ⚙️ Execução

### Comando:

```bash
Desafio_01.exe --output=C:\Destino --size=200
```

### Exemplo de saída:

```
Local de destino: C:\Destino

O diretório existe.

Tamanho escolhido: 400MB

-------------------------------------------------------------------------------------------

Iniciando geração dos dados...
Objetos criados: 18648000
Gerando arquivos temporários [##################################################] 100%
Combinando arquivos temporários [##################################################] 100%
Tamanho final do arquivo: 400,14MB

-------------------------------------------------------------------------------------------

Arquivo JSON criado com sucesso!

Tempo gasto: 00:00:10.139
```

---

## 🧰 Tecnologias Utilizadas

* .NET 8
* C#
* System.Text.Json
* System.Diagnostics
* Programação Paralela (`Parallel.For`)
* Visual Studio

---

## 📄 Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.
