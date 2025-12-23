using Godot;

[GlobalClass]
public partial class AudioManager: Node
{
    [Export] AudioStreamPlayer3D player1Audio;
    public void PlayTypingGameWrongKeySound()
    {
        player1Audio.Play();
    }
}