#!/bin/sh

cd "$(dirname "$0")"

DATE=$(date +"%Y-%m-%d %H:%M:%S")
GITTAG=$(git describe --tags --exact-match HEAD)
GITHASH=$(git rev-parse HEAD)

(
    cat <<EOF
namespace cipherc;
public static partial class ENV
{
#pragma warning disable CS0414
#pragma warning disable IDE0051
    private static readonly string _genGitTag = "$GITTAG";
    private static readonly string _genGitHash = "$GITHASH";
    private static readonly string _genDate = "$DATE";
}
EOF
) > ENV.gen.cs
