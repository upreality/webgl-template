﻿namespace Core.Sound.domain
{
    public interface ISoundPrefsRepository
    {
        void SetSoundEnabledState(bool enabled);
        bool GetSoundEnabledState();
    }
}