//////using System.Formats.Asn1;
//////using System.Text;
//////using System.Text.Json;

//////namespace Desafio_01
//////{
//////    public class GeradorArquivoJSON
//////    {
//////        private string GerarLinha(GeradorStringAlfanumerico geradorStringAlfanumerico)
//////        {
//////            JsonSerializerOptions identacao = new() { WriteIndented = true };
//////            var parametros = new ParametrosJSON
//////            {
//////                A = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
//////                B = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
//////                C = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
//////                D = geradorStringAlfanumerico.GetAlfanumericoAleatoria()
//////            };
//////            var linha = JsonSerializer.Serialize(parametros, identacao);
//////            return linha;
//////        }

//////        private void BarraDeProgresso(int quantidadeLoop, int i, out int porcentagem, out string barraDeProgresso)
//////        {
//////            porcentagem = (int)(((double)i / quantidadeLoop) * 100);
//////            barraDeProgresso = "[" + new string('#', porcentagem / 2) + new string('-', 50 - porcentagem / 2) + "]";
//////        }

//////        private double TamanhoArquivoDuranteLoop(GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop, ref long tamanhoBytes, int i)
//////        {
//////            tamanhoBytes += Encoding.UTF8.GetByteCount(GerarLinha(geradorStringAlfanumerico) + (i < quantidadeLoop - 1 ? "," : ""));
//////            double jsonEmMB = (double)tamanhoBytes / (1024 * 1024);
//////            double jsonEmMbFormatado = Math.Round(jsonEmMB, 2);
//////            return jsonEmMbFormatado;
//////        }

//////        private long VerTamanhoArquivoAposFechar(string pastaDestino)
//////        {
//////            long tamanhoBytes = new FileInfo(pastaDestino).Length;
//////            double jsonEmMbAposFechar = (double)tamanhoBytes / (1024 * 1024);
//////            double jsonEmMbFormatadoAposFechar = Math.Round(jsonEmMbAposFechar, 2);

//////            if (jsonEmMbFormatadoAposFechar < 1000.00)
//////            {
//////                Console.WriteLine($"\nTamanho do arquivo (Após fechar): {jsonEmMbFormatadoAposFechar}MB");
//////            }
//////            else if (jsonEmMbFormatadoAposFechar >= 1000.00)
//////            {
//////                double jsonEmGbFormatadoAposFechar = Math.Round((jsonEmMbFormatadoAposFechar / 1000), 2);
//////                Console.WriteLine($"\nTamanho do arquivo (Após fechar): {jsonEmGbFormatadoAposFechar}GB");
//////            }
//////            return tamanhoBytes;
//////        }

//////        private void EscreverArquivo(string pastaDestino, GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop, double limiteComTolerancia)
//////        {
//////            try
//////            {
//////                using StreamWriter writer = new(pastaDestino);
//////                int quantidadeObjetos = 0;
//////                long tamanhoBytes = 0;
//////                double tamanhoArquivoDuranteLoop = 0;
//////                string separador = "\n------------------------------------------------------------------------------------------------------------------------";

//////                Console.WriteLine(separador);
//////                Console.WriteLine("\nIniciando operação...\n");

//////                writer.WriteLine("[");

//////                List<string> linhas = new List<string>(new string[quantidadeLoop]);

//////                Parallel.For(0, quantidadeLoop, i =>
//////                {
//////                    linhas[i] = GerarLinha(geradorStringAlfanumerico);
//////                });

//////                for (int i = 0; i < quantidadeLoop; i++)
//////                {
//////                    if (tamanhoArquivoDuranteLoop <= limiteComTolerancia)
//////                    {
//////                        string linha = linhas[i];
//////                        writer.WriteLine(linha + (i < quantidadeLoop - 1 ? "," : ""));

//////                        BarraDeProgresso(quantidadeLoop, i, out int porcentagem, out string barraDeProgresso);
//////                        tamanhoArquivoDuranteLoop = TamanhoArquivoDuranteLoop(geradorStringAlfanumerico, quantidadeLoop, ref tamanhoBytes, i);

//////                        quantidadeObjetos += 4;
//////                        Console.Write($"\r{barraDeProgresso} {porcentagem}% | Objetos criados: {quantidadeObjetos} | Tamanho arquivo JSON: {tamanhoArquivoDuranteLoop}MB");
//////                    }
//////                    else
//////                    {
//////                        Console.WriteLine($"\n\nArquivo passou do limite de {limiteComTolerancia}MB, fechando arquivo.");
//////                        break;
//////                    }
//////                }

//////                writer.WriteLine("]");

//////                Console.ForegroundColor = ConsoleColor.Green;
//////                Console.WriteLine("\n\nOperação concluída!");
//////                Console.ResetColor();
//////                Console.WriteLine(separador);

//////                tamanhoBytes = VerTamanhoArquivoAposFechar(pastaDestino);
//////            }
//////            catch (Exception ex)
//////            {
//////                Console.WriteLine($"\nException: {ex.Message}");
//////            }
//////        }

//////        public void GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
//////        {
//////            GeradorStringAlfanumerico geradorStringAlfanumerico = new();

//////            double limiteMaximoEmMB = 400.00;
//////            double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);
//////            int loopEquivalente1MB = 13500;
//////            int quantidadeLoop = tamanhoArquivoDesejado * loopEquivalente1MB;

//////            if (File.Exists(pastaDestino) && tamanhoArquivoDesejado < limiteComTolerancia)
//////            {
//////                EscreverArquivo(pastaDestino, geradorStringAlfanumerico, quantidadeLoop, limiteComTolerancia);
//////                Console.WriteLine("\nArquivo JSON atualizado.");
//////            }
//////            else if (!File.Exists(pastaDestino) && tamanhoArquivoDesejado < limiteComTolerancia)
//////            {
//////                EscreverArquivo(pastaDestino, geradorStringAlfanumerico, quantidadeLoop, limiteComTolerancia);
//////                Console.WriteLine("\nArquivo JSON criado.");
//////            }
//////            else
//////            {
//////                Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteComTolerancia}MB.");
//////            }
//////        }
//////    }
//////}

////using Desafio_01;
////using System.Text;
////using System.Text.Json;

////public class GeradorArquivoJSON
////{
////    private string GerarLinha(GeradorStringAlfanumerico geradorStringAlfanumerico)
////    {
////        JsonSerializerOptions identacao = new() { WriteIndented = true };
////        var parametros = new ParametrosJSON
////        {
////            A = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
////            B = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
////            C = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
////            D = geradorStringAlfanumerico.GetAlfanumericoAleatoria()
////        };
////        return JsonSerializer.Serialize(parametros, identacao);
////    }

////    private void BarraDeProgresso(int quantidadeLoop, int i, out int porcentagem, out string barraDeProgresso)
////    {
////        porcentagem = (int)(((double)i / quantidadeLoop) * 100);
////        barraDeProgresso = "[" + new string('#', porcentagem / 2) + new string('-', 50 - porcentagem / 2) + "]";
////    }

////    private double TamanhoArquivoDuranteLoop(string linha, int quantidadeLoop, ref long tamanhoBytes, int i)
////    {
////        tamanhoBytes += Encoding.UTF8.GetByteCount(linha + (i < quantidadeLoop - 1 ? "," : ""));
////        return Math.Round((double)tamanhoBytes / (1024 * 1024), 2);
////    }

////    private long VerTamanhoArquivoAposFechar(string pastaDestino)
////    {
////        long tamanhoBytes = new FileInfo(pastaDestino).Length;
////        double jsonEmMb = Math.Round((double)tamanhoBytes / (1024 * 1024), 2);

////        if (jsonEmMb < 1000.00)
////            Console.WriteLine($"\nTamanho do arquivo (Após fechar): {jsonEmMb}MB");
////        else
////            Console.WriteLine($"\nTamanho do arquivo (Após fechar): {Math.Round(jsonEmMb / 1000, 2)}GB");

////        return tamanhoBytes;
////    }

////    private void EscreverArquivo(string pastaDestino, GeradorStringAlfanumerico geradorStringAlfanumerico, int quantidadeLoop, double limiteComTolerancia)
////    {
////        try
////        {
////            int partes = Environment.ProcessorCount;
////            int tamanhoParte = quantidadeLoop / partes;
////            List<List<string>> blocos = new();

////            Parallel.For(0, partes, i =>
////            {
////                List<string> bloco = new();
////                for (int j = 0; j < tamanhoParte; j++)
////                {
////                    int indexGlobal = i * tamanhoParte + j;
////                    bloco.Add(GerarLinha(geradorStringAlfanumerico));
////                }
////                lock (blocos)
////                {
////                    blocos.Add(bloco);
////                }
////            });

////            List<string> todasLinhas = blocos.SelectMany(b => b).ToList();
////            if (todasLinhas.Count < quantidadeLoop)
////            {
////                for (int i = todasLinhas.Count; i < quantidadeLoop; i++)
////                    todasLinhas.Add(GerarLinha(geradorStringAlfanumerico));
////            }

////            using StreamWriter writer = new(pastaDestino);
////            long tamanhoBytes = 0;
////            int quantidadeObjetos = 0;

////            Console.WriteLine("\n------------------------------------------------------------------------------------------------------------------------");
////            Console.WriteLine("\nIniciando operação...\n");

////            writer.WriteLine("[");
////            for (int i = 0; i < todasLinhas.Count; i++)
////            {
////                string linha = todasLinhas[i];
////                double tamanhoAtual = TamanhoArquivoDuranteLoop(linha, quantidadeLoop, ref tamanhoBytes, i);

////                if (tamanhoAtual <= limiteComTolerancia)
////                {
////                    writer.WriteLine(linha + (i < todasLinhas.Count - 1 ? "," : ""));
////                    BarraDeProgresso(quantidadeLoop, i, out int porcentagem, out string barra);
////                    quantidadeObjetos += 4;
////                    Console.Write($"\r{barra} {porcentagem}% | Objetos criados: {quantidadeObjetos} | Tamanho arquivo JSON: {tamanhoAtual}MB");
////                }
////                else
////                {
////                    Console.WriteLine($"\n\nArquivo passou do limite de {limiteComTolerancia}MB, fechando arquivo.");
////                    break;
////                }
////            }
////            writer.WriteLine("]");

////            Console.ForegroundColor = ConsoleColor.Green;
////            Console.WriteLine("\n\nOperação concluída!");
////            Console.ResetColor();
////            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");

////            VerTamanhoArquivoAposFechar(pastaDestino);
////        }
////        catch (Exception ex)
////        {
////            Console.WriteLine($"\nException: {ex.Message}");
////        }
////    }

////    public void GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
////    {
////        GeradorStringAlfanumerico geradorStringAlfanumerico = new();
////        double limiteMaximoEmMB = 400.00;
////        double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);
////        int loopEquivalente1MB = 13500;
////        int quantidadeLoop = tamanhoArquivoDesejado * loopEquivalente1MB;

////        if (tamanhoArquivoDesejado < limiteComTolerancia)
////        {
////            EscreverArquivo(pastaDestino, geradorStringAlfanumerico, quantidadeLoop, limiteComTolerancia);
////            Console.WriteLine(File.Exists(pastaDestino) ? "\nArquivo JSON atualizado." : "\nArquivo JSON criado.");
////        }
////        else
////        {
////            Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteComTolerancia}MB.");
////        }
////    }
////}

//using Desafio_01;
//using System.Text;
//using System.Text.Json;

//public class GeradorArquivoJSON
//{
//    private string GerarLinha(GeradorStringAlfanumerico geradorStringAlfanumerico)
//    {
//        JsonSerializerOptions identacao = new() { WriteIndented = true };
//        var parametros = new ParametrosJSON
//        {
//            A = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
//            B = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
//            C = geradorStringAlfanumerico.GetAlfanumericoAleatoria(),
//            D = geradorStringAlfanumerico.GetAlfanumericoAleatoria()
//        };
//        return JsonSerializer.Serialize(parametros, identacao);
//    }

//    private void BarraDeProgresso(int quantidadeLoop, int i, out int porcentagem, out string barraDeProgresso)
//    {
//        porcentagem = (int)(((double)i / quantidadeLoop) * 100);
//        barraDeProgresso = "[" + new string('#', porcentagem / 2) + new string('-', 50 - porcentagem / 2) + "]";
//    }

//    private double TamanhoArquivoDuranteLoop(string linha, int quantidadeLoop, ref long tamanhoBytes, int i)
//    {
//        tamanhoBytes += Encoding.UTF8.GetByteCount(linha + (i < quantidadeLoop - 1 ? "," : ""));
//        return Math.Round((double)tamanhoBytes / (1024 * 1024), 2);
//    }

//    private long VerTamanhoArquivoAposFechar(string pastaDestino)
//    {
//        long tamanhoBytes = new FileInfo(pastaDestino).Length;
//        double jsonEmMb = Math.Round((double)tamanhoBytes / (1024 * 1024), 2);

//        if (jsonEmMb < 1000.00)
//            Console.WriteLine($"\nTamanho do arquivo (Após fechar): {jsonEmMb}MB");
//        else
//            Console.WriteLine($"\nTamanho do arquivo (Após fechar): {Math.Round(jsonEmMb / 1000, 2)}GB");

//        return tamanhoBytes;
//    }

//    private void EscreverArquivo(string pastaDestino, GeradorStringAlfanumerico gerador, int quantidadeLoop, double limiteComTolerancia)
//    {
//        try
//        {
//            int partes = Environment.ProcessorCount;
//            int tamanhoParte = quantidadeLoop / partes;
//            string tempDir = Path.Combine(Path.GetDirectoryName(pastaDestino)!, "temp_parts");
//            Directory.CreateDirectory(tempDir);

//            Console.WriteLine("\n------------------------------------------------------------------------------------------------------------------------");
//            Console.WriteLine("\nIniciando operação com arquivos temporários e escrita concorrente...\n");

//            Parallel.For(0, partes, i =>
//            {
//                string tempFile = Path.Combine(tempDir, $"parte_{i}.json");
//                using FileStream fs = new(tempFile, FileMode.Create, FileAccess.Write, FileShare.None);
//                using StreamWriter writer = new(fs);
//                for (int j = 0; j < tamanhoParte; j++)
//                {
//                    string linha = GerarLinha(gerador);
//                    writer.WriteLine(linha + ",");
//                }
//            });

//            // Mesclar os arquivos
//            using FileStream finalStream = new(pastaDestino, FileMode.Create, FileAccess.Write, FileShare.None);
//            using StreamWriter finalWriter = new(finalStream);

//            finalWriter.WriteLine("[");
//            var arquivos = Directory.GetFiles(tempDir).OrderBy(f => f);
//            long tamanhoBytes = 0;
//            int iGlobal = 0;
//            int quantidadeObjetos = 0;

//            foreach (var arquivo in arquivos)
//            {
//                string[] linhas = File.ReadAllLines(arquivo);
//                foreach (var linha in linhas)
//                {
//                    string linhaFinal = linha.TrimEnd(',');
//                    double tamanhoAtual = TamanhoArquivoDuranteLoop(linhaFinal, quantidadeLoop, ref tamanhoBytes, iGlobal);

//                    if (tamanhoAtual <= limiteComTolerancia)
//                    {
//                        finalWriter.WriteLine(linhaFinal + (iGlobal < quantidadeLoop - 1 ? "," : ""));
//                        BarraDeProgresso(quantidadeLoop, iGlobal, out int porcentagem, out string barra);
//                        quantidadeObjetos += 4;
//                        Console.Write($"\r{barra} {porcentagem}% | Objetos criados: {quantidadeObjetos} | Tamanho arquivo JSON: {tamanhoAtual}MB");
//                        iGlobal++;
//                    }
//                    else
//                    {
//                        Console.WriteLine($"\n\nArquivo passou do limite de {limiteComTolerancia}MB, fechando arquivo.");
//                        goto Fim;
//                    }
//                }
//            }

//        Fim:
//            finalWriter.WriteLine("]");
//            finalWriter.Flush();
//            Directory.Delete(tempDir, true);

//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine("\n\nOperação concluída!");
//            Console.ResetColor();
//            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");

//            VerTamanhoArquivoAposFechar(pastaDestino);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"\nException: {ex.Message}");
//        }
//    }

//    public void GerarArquivoJson(string pastaDestino, int tamanhoArquivoDesejado)
//    {
//        GeradorStringAlfanumerico gerador = new();
//        double limiteMaximoEmMB = 400.00;
//        double limiteComTolerancia = limiteMaximoEmMB + (limiteMaximoEmMB / 100);
//        int loopEquivalente1MB = 13500;
//        int quantidadeLoop = tamanhoArquivoDesejado * loopEquivalente1MB;

//        if (tamanhoArquivoDesejado < limiteComTolerancia)
//        {
//            EscreverArquivo(pastaDestino, gerador, quantidadeLoop, limiteComTolerancia);
//            Console.WriteLine(File.Exists(pastaDestino) ? "\nArquivo JSON atualizado." : "\nArquivo JSON criado.");
//        }
//        else
//        {
//            Console.WriteLine($"\nArquivo não gerado, passou do limite de {limiteComTolerancia}MB.");
//        }
//    }
//}

