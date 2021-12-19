using System;
using System.Collections.Generic;
using System.Linq;

namespace Algoritmo_genetico {
    class Program {
        const int INDIVIDUOS_POR_GERACAO = 10000;
        const int NUMERO_DE_GERACOES = 1000;
        const int NUMERO_DE_CLASSIFICADOS = 100;
        const int TAXA_DE_MUTACAO_EM_PORCENTAGEM = 10;

        static double problema(double x, double y) {
            return Math.Pow(x, 13) - Math.Pow(y, -2) + 132;
        }
        
        static double fitness(double x, double y) {
            double resultado_da_equacao = problema(x, y);
            if (resultado_da_equacao == 0) {
                return 999999999999;
            }
            double resultado_fitness = 1/resultado_da_equacao;
            return Math.Abs(resultado_fitness); 
        } 

        static double obterNumeroAleatorio(Random random, double min, double max) {
            return random.NextDouble() * (max - min) + min;
        }
        
        static List<Tuple<double, double>> solucoesIniciais(Random random, double min, double max) {
            var solucoes = new List<Tuple<double, double>>();
             for(int i=0; i < INDIVIDUOS_POR_GERACAO; i++) {
                solucoes.Add(Tuple.Create(obterNumeroAleatorio(random, min, max), obterNumeroAleatorio(random, min, max)));
            }
            return solucoes;
        }

        static List<Tuple<double, Tuple<double, double>>> GeracaoDosIndividuos(List<Tuple<double, double>> solucoes) {
            var individuos = new List<Tuple<double, Tuple<double, double>>>();
            for(int numeroIndividuo=0; numeroIndividuo < INDIVIDUOS_POR_GERACAO; numeroIndividuo++) {
                individuos.Add(Tuple.Create(fitness(solucoes[numeroIndividuo].Item1, solucoes[numeroIndividuo].Item2), solucoes[numeroIndividuo]));              
            }
            return individuos;
        }


        static Tuple<double,Tuple<double, double>> melhorIndividuoDaGeracao(List<Tuple<double, Tuple<double, double>>> individuos) {
            return individuos[0];
        }

        static void imprimirInformacoesDoMelhorIndividuoNaTela(int geracao, Tuple<double,Tuple<double, double>> individuo) {
            
            String titulo = $"Geração {geracao}";
            String valorXY = $"x e y: {individuo.Item2, 10:N0}";
            String resultadoFitness = $"resultado da fitness: {individuo.Item1}";
            String resultadoExpressao = $"resultado expressão: {problema(individuo.Item2.Item1, individuo.Item2.Item2)}";

            int maiorNumeroDeLetras = Math.Max(Math.Max(titulo.Length, valorXY.Length), Math.Max(resultadoFitness.Length, resultadoFitness.Length));
            int maior = maiorNumeroDeLetras % 2 == 0 ? maiorNumeroDeLetras : maiorNumeroDeLetras+1; 

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"|{new string('-', maior)}|");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" {new string('-', (maior-titulo.Length)/2)}{titulo}{new string('-', (maior-titulo.Length)/2)} ");
            Console.WriteLine($" {new string(' ', (maior-valorXY.Length)/2)}{valorXY}{new string('-', (maior-valorXY.Length)/2)} ");
            Console.WriteLine($" {resultadoFitness}{new string(' ', (maior-resultadoFitness.Length)/2)} ");
            Console.WriteLine($" {resultadoExpressao}{new string(' ', (maior-resultadoExpressao.Length)/2)} ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"|{new string('-', maior)}|\n");
        }

        static List<Tuple<double, double>> valoresParaNovaGeracao(List<Tuple<double, Tuple<double, double>>> Resultadoindividuos) {
            var valores = new List<Tuple<double, double>>();

            for(int j=0; j< NUMERO_DE_CLASSIFICADOS; j++) {
                valores.Add(Tuple.Create(Resultadoindividuos[j].Item2.Item1, Resultadoindividuos[j].Item2.Item2));
            }
            return valores;
        }

        static List<Tuple<double, double>> novaGeracao(Random random, List<Tuple<double, double>> DNAIndividuos, Tuple<double, double> melhorDaGeracao) {
            var novaGen = new List<Tuple<double, double>>();

            novaGen.Add(melhorDaGeracao);

            for(int j=0; j < INDIVIDUOS_POR_GERACAO-1; j++) {
                double v1 = DNAIndividuos[random.Next(0, NUMERO_DE_CLASSIFICADOS)].Item1 * obterNumeroAleatorio(random, 100-TAXA_DE_MUTACAO_EM_PORCENTAGEM, 100+TAXA_DE_MUTACAO_EM_PORCENTAGEM)/100;

                double v2 = DNAIndividuos[random.Next(0, NUMERO_DE_CLASSIFICADOS)].Item2 * obterNumeroAleatorio(random, 100-TAXA_DE_MUTACAO_EM_PORCENTAGEM, 100+TAXA_DE_MUTACAO_EM_PORCENTAGEM)/100;
                novaGen.Add(Tuple.Create(v1, v2));
            }
            return novaGen;
        }

        static void Main(string[] args) {
            
            Random random = new Random();
            var solucoes = solucoesIniciais(random, 0.0, 1000.0);
    
            for(int geracao=0; geracao < NUMERO_DE_GERACOES; geracao++) {
                var resultadosDosIndividuos = GeracaoDosIndividuos(solucoes);
                var resultadoOrganizado = resultadosDosIndividuos.OrderBy(i => i.Item1).Reverse().ToList();
                var melhorIndividuo = melhorIndividuoDaGeracao(resultadoOrganizado);
                imprimirInformacoesDoMelhorIndividuoNaTela(geracao, melhorIndividuo);

                var DNAIndividuos = valoresParaNovaGeracao(resultadoOrganizado);
                var novasSolucoes = novaGeracao(random, DNAIndividuos, melhorIndividuo.Item2);
                solucoes = novasSolucoes;
    }
  }
}
}