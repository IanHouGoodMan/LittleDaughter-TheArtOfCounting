# Counting Art

A free, open-source, browser-based lab for **counting problems** — from primary-school
"how many triangles are in this figure?" puzzles to the first chapters of an undergraduate
combinatorics course.

Every object being counted is enumerated and shown on screen, so learners can check their own
count by category, find what they missed, and discover the formula from a table of results
("counting in two ways").

Available in **中文 · English · Français**.

**Try it:** <https://ianhougoodman.github.io/LittleDaughter-TheArtOfCounting/>

## Module 1: Count the triangles

- Click three corners to "catch" a triangle on a triangular grid.
- **Check my work** shows, for every kind of triangle (side length × pointing up/down),
  whether some are still missing — without giving away the answer.
- **Show answers** steps through every triangle and marks the ones you missed.
- A pattern table shows how the total grows with the figure (e.g. 3 rows of width *n*: 12*n* − 8).

Shapes: parallelogram strip, big triangle, trapezoid.

## Project layout

| Path | Purpose |
|---|---|
| `src/CountingArt.Core` | Pure C# counting engine (no UI dependencies) |
| `src/CountingArt.Web` | Blazor WebAssembly front end (installable PWA, works offline) |
| `tests/CountingArt.Core.Tests` | xUnit tests; brute-force counts are checked against closed-form formulas |
| `docs/discussion-2026-11-18.md` | Project discussion notes (in Chinese): motivation, tech choice, outreach |
| `docs/references/` | Reference material, including *Counting Rocks!* (CC BY 4.0) |

## Run locally (Windows, macOS, Linux)

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet test tests/CountingArt.Core.Tests
dotnet run --project src/CountingArt.Web
```

Then open the URL printed in the terminal (http://localhost:5153).

## Deployment

Every push to `main` runs the tests and publishes the site to GitHub Pages
(`.github/workflows/pages.yml`). In the repository settings, set
**Pages → Build and deployment → Source** to **GitHub Actions**.

## Roadmap

1. Triangle counting with category feedback ✅
2. More modules: rectangles in a grid, handshakes, lattice paths & Pascal's triangle,
   staircases & Fibonacci, Tower of Hanoi
3. Printable worksheets with answer keys
4. Optional alignment with open combinatorics textbooks (with the authors' permission)

## License

[MIT](LICENSE) for the code.

`docs/references/CountingRocks-2025-05-04.pdf` is *Counting Rocks! An Introduction to Combinatorics*
by Henry Adams, Kelly Emmrich, Maria Gillespie, Shannon Golden, and Rachel Pries, licensed under
[CC BY 4.0](https://creativecommons.org/licenses/by/4.0/) and redistributed unmodified.
