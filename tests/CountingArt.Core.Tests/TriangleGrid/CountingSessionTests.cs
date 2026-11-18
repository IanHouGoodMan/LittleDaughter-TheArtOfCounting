using CountingArt.Core.TriangleGrid;

namespace CountingArt.Core.Tests.TriangleGrid;

public class CountingSessionTests
{
    private static CountingSession NewSession() => new(TriangleGridFigure.Triangle(2));

    [Fact]
    public void Submit_NewTriangle_ThenSameTriangleInAnotherOrder_IsAlreadyFound()
    {
        var session = NewSession();

        Assert.Equal(SubmissionResult.NewTriangle, session.Submit(new(0, 0), new(2, 0), new(0, 2), out var big));
        Assert.Equal(GridTriangle.Up(0, 0, 2), big);
        Assert.Equal(SubmissionResult.AlreadyFound, session.Submit(new(0, 2), new(0, 0), new(2, 0), out _));
        Assert.Single(session.Found);
    }

    [Fact]
    public void Submit_TriangleOutsideTheFigure_ReportsSidesNotDrawn()
    {
        var session = NewSession();

        Assert.Equal(SubmissionResult.SidesNotDrawn, session.Submit(GridTriangle.Up(0, 0, 3)));
        Assert.Equal(SubmissionResult.SidesNotDrawn, session.Submit(GridTriangle.Down(1, 0)));
        Assert.Empty(session.Found);
    }

    [Fact]
    public void Submit_PointsThatDoNotMakeAGridTriangle_ReportsNotAGridTriangle()
    {
        var session = NewSession();

        Assert.Equal(SubmissionResult.NotAGridTriangle, session.Submit(new(0, 0), new(2, 0), new(0, 1), out _));
    }

    [Fact]
    public void Progress_ShowsWhichKindsAreStillMissing_AndCompletesWhenAllAreFound()
    {
        var session = NewSession();
        session.Submit(GridTriangle.Up(0, 0));
        session.Submit(GridTriangle.Up(0, 0, 2));

        var progress = session.Progress();

        CategoryProgress[] expected =
        [
            new(new(Orientation.Up, 1), 1, 3),
            new(new(Orientation.Down, 1), 0, 1),
            new(new(Orientation.Up, 2), 1, 1),
        ];
        Assert.Equal(expected, progress);
        Assert.False(session.IsComplete);

        foreach (var t in session.AllTriangles)
        {
            session.Submit(t);
        }

        Assert.True(session.IsComplete);
        Assert.All(session.Progress(), p => Assert.True(p.IsComplete));
    }
}
