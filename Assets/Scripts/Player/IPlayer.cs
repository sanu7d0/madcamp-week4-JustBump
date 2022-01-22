using System;


public interface IPlayer
{
    public int id { get; set; }
    public int score { get; set; }
    public bool isDead { get; set; }
}
