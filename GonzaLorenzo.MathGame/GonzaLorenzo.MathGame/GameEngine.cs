﻿using System;

namespace GonzaLorenzo.MathGame;

internal class GameEngine
{
    private static readonly Random random = new();

    int firstNumber = 0;
    int secondNumber = 0;

    int randomMinNumber = 0;
    int randomMaxNumber = 0;

    int randomMinDividend = 0;
    int randomMaxDividend = 0;

    int randomMinDivisor = 0;
    int randomMaxDivisor = 0;

    internal void MathGame(GameMode gameMode)
    {
        Difficulty difficulty = ChooseDifficulty();
        int scoreValue = GetScoreValue(difficulty);
        SetNumbersRange(difficulty);

        string operation = SetOperation(gameMode);
        int questions = ChooseNumberOfQuestions();

        int score = 0;

        DateTime startTime = DateTime.Now;

        for (int i = 0; i < questions; i++)
        {
            Console.Clear();

            if (gameMode == GameMode.Random)
            {
                operation = SetOperation(gameMode);
            }

            GetRandomNumbers(operation);

            Console.WriteLine($"Question {i + 1}. What is {firstNumber} {operation} {secondNumber}?");

            string result = Console.ReadLine();

            while (!int.TryParse(result, out _))
            {
                Console.Clear();
                Console.WriteLine($"Invalid input. What is {firstNumber} {operation} {secondNumber}?");
                result = Console.ReadLine();
            }

            if (CheckResults(int.Parse(result), operation, firstNumber, secondNumber))
            {
                score += scoreValue;
                SendNextQuestion(true, i, questions);
            }
            else
            {
                SendNextQuestion(false, i, questions);
            }
        }

        DateTime endTime = DateTime.Now;
        TimeSpan timeTaken = endTime - startTime;
        Helpers.AddToHistory(gameMode, difficulty, questions, score, timeTaken);
        Console.WriteLine($"Game over. Your final score is {score}. Press enter to go back to the menu.");
        Console.ReadLine();
    }

    internal void SetGameMode(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.Addition:
                Console.Clear();
                Console.WriteLine("Addition game selected.");
                break;

            case GameMode.Substraction:
                Console.Clear();
                Console.WriteLine("Substraction game selected.");
                break;

            case GameMode.Multiplication:
                Console.Clear();
                Console.WriteLine("Multiplication game selected.");
                break;

            case GameMode.Division:
                Console.Clear();
                Console.WriteLine("Division game selected.");
                break;

            case GameMode.Random:
                Console.Clear();
                Console.WriteLine("Random game selected.");
                break;
        }

        MathGame(gameMode);
    }

    string SetOperation(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.Addition:
                return "+";

            case GameMode.Substraction:
                return "-";

            case GameMode.Multiplication:
                return "*";

            case GameMode.Division:
                return "/";

            case GameMode.Random:
                return SetRandomOperation();

            default:
                return "+";
        }

    }

    string SetRandomOperation()
    {
        switch (random.Next(1, 5))
        {
            case 1:
                return "+";

            case 2:
                return "-";

            case 3:
                return "*";

            case 4:
                return "/";

            default:
                return "+";
        }
    }

    int GetScoreValue(Difficulty difficulty)
    {
        int scoreValue = 0;

        switch (difficulty)
        {
            case Difficulty.Easy:
                scoreValue = 1;
                break;

            case Difficulty.Medium:
                scoreValue = 2;
                break;

            case Difficulty.Hard:
                scoreValue = 3;
                break;
        }

        return scoreValue;
    }

    Difficulty ChooseDifficulty()
    {
        Console.WriteLine("Please choose your difficulty. Easy, medium or hard.");

        Difficulty validDifficulty = Difficulty.Easy;
        bool validInput = false;

        while (!validInput)
        {
            string difficulty = Console.ReadLine();

            switch (difficulty.Trim().ToLower())
            {
                case "easy":
                    Console.Clear();
                    Console.WriteLine("Easy difficulty selected.");
                    validDifficulty = Difficulty.Easy;
                    validInput = true;
                    break;

                case "medium":
                    Console.Clear();
                    Console.WriteLine("Medium difficulty selected.");
                    validDifficulty = Difficulty.Medium;
                    validInput = true;
                    break;

                case "hard":
                    Console.Clear();
                    Console.WriteLine("Hard difficulty selected.");
                    validDifficulty = Difficulty.Hard;
                    validInput = true;
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input. Please choose your difficulty. Easy, medium or hard.");
                    break;
            }

        }

        return validDifficulty;
    }

    int ChooseNumberOfQuestions()
    {
        Console.WriteLine("Please choose the number of questions from 1 to 10.");

        bool isValid = false;
        int numberOfQuestions = 0;

        while (!isValid)
        {
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out numberOfQuestions) || numberOfQuestions < 1 || numberOfQuestions > 10)
            {
                Console.Clear();
                Console.WriteLine("Invalid input. Please choose the number of questions from 1 to 10.");
            }
            else
            {
                isValid = true;
                numberOfQuestions = int.Parse(input);
            }
        }

        Console.Clear();
        if (numberOfQuestions == 1)
        {
            Console.WriteLine($"{numberOfQuestions} question selected. Press enter to start the game.");
        }
        else
        {
            Console.WriteLine($"{numberOfQuestions} questions selected. Press enter to start the game.");
        }

        Console.ReadLine();

        return numberOfQuestions;
    }

    void GetRandomNumbers(string operation)
    {
        if (operation != "/")
        {
            firstNumber = random.Next(randomMinNumber, randomMaxNumber);
            secondNumber = random.Next(randomMinNumber, randomMaxNumber);
        }
        else
        {
            firstNumber = random.Next(randomMinDividend, randomMaxDividend);
            secondNumber = random.Next(randomMinDivisor, randomMaxDivisor);

            while (firstNumber % secondNumber != 0)
            {
                firstNumber = random.Next(randomMinDividend, randomMaxDividend);
                secondNumber = random.Next(randomMinDivisor, randomMaxDivisor);
            }
        }
    }

    void SetNumbersRange(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                randomMinNumber = 1;
                randomMaxNumber = 10;

                randomMinDividend = 10;
                randomMaxDividend = 100;

                randomMinDivisor = 2;
                randomMaxDivisor = 10;
                break;

            case Difficulty.Medium:
                randomMinNumber = 10;
                randomMaxNumber = 100;
                randomMinDividend = 100;
                randomMaxDividend = 1000;

                randomMinDivisor = 2;
                randomMaxDivisor = 10;
                break;

            case Difficulty.Hard:
                randomMinNumber = 100;
                randomMaxNumber = 1000;
                randomMinDividend = 100;
                randomMaxDividend = 1000;

                randomMinDivisor = 10;
                randomMaxDivisor = 100;
                break;
        }
    }

    bool CheckResults(int result, string operation, int firstNumber, int secondNumber)
    {
        switch (operation)
        {
            case "+":
                return result == firstNumber + secondNumber;

            case "-":
                return result == firstNumber - secondNumber;

            case "*":
                return result == firstNumber * secondNumber;

            case "/":
                return result == firstNumber / secondNumber;

            default:
                throw new ArgumentException("Invalid operation");
        }
    }

    void SendNextQuestion(bool isCorrect, int i, int questions)
    {
        if (isCorrect)
        {
            if (i == questions - 1)
            {
                Console.WriteLine("Your answer was correct!");
                return;
            }

            Console.WriteLine("Your answer was correct! Press enter for the next question.");
            Console.ReadLine();
        }
        else
        {
            if (i == questions - 1)
            {
                Console.WriteLine("Your answer was incorrect.");
                return;
            }

            Console.WriteLine("Your answer was incorrect. Press enter key for the next question.");
            Console.ReadLine();
        }
    }
}
