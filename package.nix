{
  pkgs ? import <nixpkgs> { },
  ...
}:

with pkgs;
buildDotnetModule {
  pname = "TicTacToe";
  version = "0.1";

  src = ./.;

  dotnet-sdk = dotnetCorePackages.sdk_9_0;
  dotnet-runtime = dotnetCorePackages.runtime_9_0;

  executables = [ "TicTacToe" ];
}
