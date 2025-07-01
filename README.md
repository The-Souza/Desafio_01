<h1 align="center">
   Gerador de Arquivo JSON
</h1>

<p align="center">
    <img alt="Static Badge" src="https://img.shields.io/badge/LICENSE-MIT-blue">
    <img alt="Static Badge" src="https://img.shields.io/badge/STATUS-EM_DESENVOLVIMENTO-green">
</p>

<h2>
    Descrição do Projeto
</h2>

<ol>
    <li>
        <strong>Objetivo</strong>
        <p>
            Desenvolvido usando Console Application em C# que gera um arquivo no formato JSON
            contendo uma coleção de objetos com exatamente 4 propriedades (A, B, C e D). 
            Cada propriedade é preenchida com uma string alfanumérica aleatória. 
        <p>
    </li>
    <li>
        <strong>Especificações</strong>
        <ul>
            <strong>2.1. Aplicação Console</strong>
            <ul>
                É necessário passar dois parâmetros: Caminho de saída (--output) e
                Tamanho alvo do arquivo, em megabytes (--size).
            </ul>
            <strong>2.2. Formato do JSON</strong>
            <ul>
                O arquivo final é estruturado como uma raiz de objetos 
                (ex: {"A": "......", "B": "......", "C": "......","D": "......"}).
            </ul>
            <strong>2.3. Dados Aleatórios</strong>
            <ul>
                Cada campo (A, B, C, D) contém ums string alfanumérica de 6 caracteres.
            </ul>
            <strong>2.4. Tamanho do Arquivo</strong>
            <ul>
                <li>
                    Gera um arquivo com o tamanho escolhido com ±1% de tolerância. 
                    O tamanho limite foi estabelecido em 400MB.
                </li>
                <li>
                    A quantidade de registros é calculada dinamicamente a cada escrita no arquivo.
                </li>
            </ul>
            <strong>2.5. Performance & Escalabilidade</strong>
            <ul>
                <li>
                    Não carrega toda a coleção em memória antes de gravar.
                </li>
                <li>
                    É utilizado técnicas de streaming de saída para minimizar consumo de RAM (ex: System.Text.Json).
                </li>
                <li>
                    No código como foi calculado o número de registros e o valor para o loop de gravação.
                </li>
            </ul>
        </ul>
    </li>
</ol>

<h2>
  Funcionalidades do Projeto
</h2>

<ul>
    <li>
        <strong>Linha de Comando</strong>:
    </li>
    <li>
        <strong>Tempo Gasto</strong>:
    </li>
    <li>
        <strong>Gerador Arquivo JSON</strong>:
    </li>
    <li>
        <strong>Gerador String Alfanumérica</strong>:
    </li>
</ul>

<h2>
  Tecnologias Utilizadas
</h2>

<ul>
    <li>
        <strong>.NET 8</strong>
    </li>
    <li>
        <strong>Visual Studio</strong>
    </li>
</ul>