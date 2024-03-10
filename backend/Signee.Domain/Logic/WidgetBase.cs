using Signee.Domain.Entities.Widget;

namespace Signee.Domain.Logic;


public abstract class WidgetBase
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public WidgetTypes Type { get; set; }
    
    // Common implementation that can be overridden by subclasses
    public virtual void Initialize(Widget model)
    {
        // Common initialization logic (if any)
    }

    public virtual bool Validate()
    {
        // Common validation logic (if any)
        return true;
    }

    public abstract Widget ToEntity(); // Still requires implementation in subclasses
}
