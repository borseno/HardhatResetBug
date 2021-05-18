# HardhatResetBug
Minimal repro for hardhat_reset bug

The issue is, that we attempt to reset to a block X (X > 0), but instead, it resets to the 0th block.

## Steps to reproduce:
1. Run hardhat with mainnet fork. Even if you don't specify block it's fine.
2. Specify your Alchemy key in the Program.fs file (10th line - alchemyKey) - replace the whole string with your key (including <>)
3. Save the file
4. Go to the root folder
5. Run "dotnet run" command
6. You should be able to see the output that prints block numbers before calling hardhat_reset and after.
