using Data.Nodes;

namespace Interfaces
{
    public interface ITextVisual
    {
        public void ShowVisuals();
        public void HideVisuals();
        public void RequestNewLine(TextNode node);
    }
}