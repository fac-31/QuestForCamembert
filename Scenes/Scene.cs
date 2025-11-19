namespace QuestForCamembert
{
    public sealed class Scene
    {
        public Scene(
            string title,
            string intro,
            string leftChoiceLabel,
            ScenePath leftPath,
            string rightChoiceLabel,
            ScenePath rightPath)
        {
            Title = title;
            Intro = intro;
            LeftChoiceLabel = leftChoiceLabel;
            LeftPath = leftPath;
            RightChoiceLabel = rightChoiceLabel;
            RightPath = rightPath;
        }

        public string Title { get; }
        public string Intro { get; }
        public string LeftChoiceLabel { get; }
        public ScenePath LeftPath { get; }
        public string RightChoiceLabel { get; }
        public ScenePath RightPath { get; }
    }
}
