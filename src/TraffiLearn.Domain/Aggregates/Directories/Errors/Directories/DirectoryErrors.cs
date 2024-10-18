﻿using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.Errors.Directories
{
    public static class DirectoryErrors
    {
        public static readonly Error EmptySections =
            Error.Validation(
                code: "Directory.EmptySections",
                description: "Sections cannot be empty.");

        public static readonly Error NotFound =
            Error.NotFound(
                code: "Directory.NotFound",
                description: "Directory has not been found.");
    }
}