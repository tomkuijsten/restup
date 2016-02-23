﻿namespace Devkoes.HttpMessage.Models.Schemas
{
    public enum MediaType
    {
        Unsupported = 0, // Will be the default(MediaType)
        JSON,
        XML,
        HTML,
        IMAGE,
        CSS
    }
}
