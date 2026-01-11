# Experiment Report: System.Drawing Support on macOS

## Problem
The test `Font_Clone` (Tests13.cs) fails on macOS with a `System.TypeInitializationException`.
**Root Cause:** The project relies on the `System.Drawing.Common` (GDI+) library. Microsoft has officially discontinued support for this library on non-Windows platforms starting with .NET 6/7.

## Attempted Solutions (in this branch)
To attempt running the tests without modifying the source code, the following steps were performed:
1. Installed the native library `mono-libgdiplus` (via Homebrew).
2. Created symbolic links (`ln -s`) for library path compatibility.
3. Added the `System.Drawing.EnableUnixSupport` configuration flag to the `.csproj` file.
4. Downgraded the `System.Drawing.Common` package to version 6.0.0.

## Result
The experiment demonstrated that on Apple Silicon architecture (M1/M2/M3) combined with the .NET 8 SDK, even forced compatibility settings result in a `PlatformNotSupportedException`.

## Recommended Solution (Architecture Proposal)
To ensure true cross-platform compatibility (Windows/Linux/macOS) and passing of all tests, it is necessary to **remove the dependency on GDI+ (System.Drawing)**.

**Refactoring Plan:**
1. Replace `System.Drawing.Common` with a cross-platform alternative:
   - **SkiaSharp** 
   - **SixLabors.ImageSharp**
2. Rewrite font and image handling methods using the new library types.
