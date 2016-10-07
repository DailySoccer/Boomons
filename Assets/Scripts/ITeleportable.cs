public interface ITeleportable
{
	void TeleportTo(Teleport target);
	bool IsTeleporting { get;  }
}

