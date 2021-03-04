﻿using System;

namespace BaseModel
{
    public class JSONReadingException: Exception
    {
        public JSONReadingException(String message) : base (message) {}

        public JSONReadingException(String message, Exception inner) : base(message,inner) {}
    }
}