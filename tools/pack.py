#!/usr/bin/env python3
"""Pack authored C# (.cs) into its L3 whitespace-packed mirror (.min.cs).

The .min.cs is the maximally-minified showcase form: comments removed, every line
stripped of indentation and joined. It is never compiled (the build excludes
**/*.min.cs); the .cs stays the source of truth. Joining only inserts a space when
two word characters would otherwise collide, so token boundaries are preserved.

    python tools/pack.py <file-or-dir> [<file-or-dir> ...]

A directory packs every *.cs under it (skipping *.min.cs, obj/, bin/, Generated/).
"""
import os
import sys


def strip_comments(src):
    out = []
    i, n = 0, len(src)
    state = "code"
    while i < n:
        c = src[i]
        nxt = src[i + 1] if i + 1 < n else ""
        if state == "code":
            if c == "/" and nxt == "/":
                state = "line"; i += 2; continue
            if c == "/" and nxt == "*":
                state = "block"; i += 2; continue
            if c == "@" and nxt == '"':
                out.append('@"'); state = "verbatim"; i += 2; continue
            if c == '"':
                out.append(c); state = "string"; i += 1; continue
            if c == "'":
                out.append(c); state = "char"; i += 1; continue
            out.append(c); i += 1; continue
        if state == "line":
            if c == "\n":
                out.append("\n"); state = "code"
            i += 1; continue
        if state == "block":
            if c == "*" and nxt == "/":
                state = "code"; i += 2; continue
            if c == "\n":
                out.append("\n")
            i += 1; continue
        if state == "string":
            out.append(c)
            if c == "\\" and i + 1 < n:
                out.append(src[i + 1]); i += 2; continue
            if c == '"':
                state = "code"
            i += 1; continue
        if state == "char":
            out.append(c)
            if c == "\\" and i + 1 < n:
                out.append(src[i + 1]); i += 2; continue
            if c == "'":
                state = "code"
            i += 1; continue
        if state == "verbatim":
            if c == '"' and nxt == '"':
                out.append('""'); i += 2; continue
            out.append(c)
            if c == '"':
                state = "code"
            i += 1; continue
    return "".join(out)


def _word(ch):
    return ch.isalnum() or ch == "_"


def pack(src):
    lines = [ln.strip() for ln in strip_comments(src).splitlines()]
    lines = [ln for ln in lines if ln]
    result = ""
    for ln in lines:
        if not result:
            result = ln
        elif _word(result[-1]) and _word(ln[0]):
            result += " " + ln
        else:
            result += ln
    return result + "\n"


def pack_file(path):
    with open(path, encoding="utf-8") as f:
        src = f.read()
    out = path[:-3] + ".min.cs"
    with open(out, "w", encoding="utf-8") as f:
        f.write(pack(src))
    print(f"packed {os.path.relpath(out)}")


def iter_cs(root):
    skip = {"obj", "bin", "Generated"}
    for dirpath, dirs, files in os.walk(root):
        dirs[:] = [d for d in dirs if d not in skip]
        for name in files:
            if name.endswith(".cs") and not name.endswith(".min.cs"):
                yield os.path.join(dirpath, name)


def main(argv):
    if not argv:
        print(__doc__); return 1
    for target in argv:
        if os.path.isdir(target):
            for path in iter_cs(target):
                pack_file(path)
        else:
            pack_file(target)
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv[1:]))
