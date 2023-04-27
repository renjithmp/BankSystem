using System;
using System.Collections.Generic;

public class MonteCarloVAR
{
    static void Main(string[] args)
    {
        // define portfolio data
        double initialInvestment = 1000000;
        double[] returns = { -0.02, -0.01, 0.03, 0.02, -0.01, -0.02, 0.02, 0.01, 0.01, -0.01 };
        double[] weights = { 0.2, 0.3, 0.1, 0.15, 0.1, 0.05, 0.05, 0.025, 0.025, 0.025 };
        double confidenceLevel = 0.95;

        // generate market scenarios
        List<double[]> scenarios = GenerateScenarios(returns, 10000);

        // calculate losses for each scenario
        List<double> losses = new List<double>();
        foreach (double[] scenario in scenarios)
        {
            double portfolioValue = CalculatePortfolioValue(initialInvestment, weights, scenario);
            double loss = initialInvestment - portfolioValue;
            losses.Add(loss);
        }

        // sort losses in ascending order
        losses.Sort();

        // calculate VAR for desired confidence level
        int index = (int)Math.Ceiling(losses.Count * (1 - confidenceLevel));
        double VAR = losses[index];

        Console.WriteLine("Value at Risk (VAR) at " + (1 - confidenceLevel) * 100 + "% confidence level: " + VAR.ToString("C"));
    }

    // function to generate market scenarios
    static List<double[]> GenerateScenarios(double[] returns, int numScenarios)
    {
        Random rand = new Random();
        List<double[]> scenarios = new List<double[]>();

        for (int i = 0; i < numScenarios; i++)
        {
            double[] scenario = new double[returns.Length];
            for (int j = 0; j < returns.Length; j++)
            {
                double randValue = rand.NextDouble() * 2 - 1;
                scenario[j] = returns[j] + randValue;
            }
            scenarios.Add(scenario);
        }

        return scenarios;
    }

    // function to calculate portfolio value
    static double CalculatePortfolioValue(double initialInvestment, double[] weights, double[] returns)
    {
        double portfolioValue = initialInvestment;
        for (int i = 0; i < weights.Length; i++)
        {
            portfolioValue += portfolioValue * weights[i] * returns[i];
        }
        return portfolioValue;
    }
}
