using UnityEngine;

namespace Extension
{
	public static class ExtendedAudioSource
	{
		public static void Play(this AudioSource source, bool playAlone = false)
		{
			if(playAlone)
				source.Stop();
			source.Play();
		}

		public static void PlayOneShot(this AudioSource source, AudioClip clip, bool playAlone = false)
		{
			if(playAlone)
				source.Stop();
			source.PlayOneShot(clip);
		}
	}
}
