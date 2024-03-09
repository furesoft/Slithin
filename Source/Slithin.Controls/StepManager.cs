using Avalonia.Controls;
using Avalonia.Labs.Controls;

namespace Slithin.Controls;

public static class StepManager
{
    private static StepBar Bar = null;

    private static Carousel Container = null;

    public static bool GetManageStepBar(StepBar target)
    {
        return Bar == target;
    }

    public static bool GetStepContainer(Carousel target)
    {
        return Container == target;
    }

    public static void Next()
    {
        Bar?.NextCommand.Execute(null);
    }

    public static void SetManageStepBar(StepBar target, bool value)
    {
        Bar = target;
    }

    public static void SetStepContainer(Carousel target, bool value)
    {
        Container = target;
    }
}
