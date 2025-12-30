using System;
using System.ComponentModel;
using Godot;
public partial class GameManager : Node
{
    [Export] public CustomerManager customerManager;
    public LevelProfile levelProfile = Levels.firstLevel;
    public double timeLeft;
    private double preLevelScore;
    private double peakScore;
    public double score;
    public bool levelFinished;
    [Export] public Label scoreLabel;
    [Export] public Label timeLeftLabel;
    [Export] public TicketManager ticketManager1;
    [Export] public TicketManager ticketManager2;
    [Export] public TicketManager ticketManager3;
    [Export] public TicketManager ticketManager4;

    public override void _Ready()
    {
        StartLevel();//TODO replace with main menu thing
    }

    public override void _Process(double delta)
    {
        if(!levelFinished)
        {
            timeLeft -= delta;
            if(timeLeft <= 0)
            {
                LostLevel();
            }

            int minutes = (int)(timeLeft / 60);
            int seconds = (int)(timeLeft % 60);
            timeLeftLabel.Text = $"{minutes:00}:{seconds:00}";
        }

    }
    public void StartLevel()
    {
        levelFinished = false;
        timeLeft = levelProfile.time;
        customerManager.StartLevel(levelProfile);
    }
    public void ResetLevel()
    {
        levelFinished = true;
        customerManager.Reset();
        SetScore(preLevelScore);
        StartLevel();
    }
    public void LostLevel()
    {
        levelFinished = true;
        //you lost screen, fade out, sound whatever
        ResetLevel();
    }
    public void WonLevel()
    {
        levelFinished = true;
        SetScore(GetScore()+timeLeft);
        //you won screen, fade out, sound whatever
        NextLevel();
    }
    public void NextLevel()
    {
        levelFinished = true;
        customerManager.Reset();
        levelProfile = levelProfile.NextLevel;
        StartLevel();
    }
    public double GetScore()
    {
        return score;
    }
    public void SetScore(double newScore)
    {
        if(newScore > peakScore)
        {
            peakScore = newScore;
        }
        score = newScore;
        scoreLabel.Text = Math.Floor(score).ToString();
    }

    public void AddOrderTicket(Order order)
    {
        ticketManager1.AddOrderTicket(order);
    }
}
