namespace NetworkGame.Content
{
    public class ContentSizeChooser
    {
        readonly float[] m_availableSizes;
        readonly float m_dpiIn1XSize;

        public readonly float SizeMultiplier;
        public readonly string SizeMultiplierPrefix;

        public ContentSizeChooser(int dpi)
        {
            this.m_dpiIn1XSize = 160;
            this.m_availableSizes = new[] { 1f, 1.5f, 2f, 3f, 4f };

            this.SizeMultiplier = CalcSizeMultiplier(dpi);
            this.SizeMultiplierPrefix = 'x' + SizeMultiplier.ToString(".#").Replace(',', '.');
        }

        float CalcSizeMultiplier(int dpi)
        {
            var sizeMultiplier = dpi / m_dpiIn1XSize;

            var firstSize = m_availableSizes[0];
            if (sizeMultiplier < firstSize)
                return firstSize;

            var lastSize = m_availableSizes[m_availableSizes.Length - 1];
            if (sizeMultiplier > lastSize)
                return lastSize;

            float leftBorder = 0f;
            float rightBorder = 0f;
            for (int i = 0; i < m_availableSizes.Length - 1; i++)
            {
                if (sizeMultiplier >= m_availableSizes[i] && sizeMultiplier <= m_availableSizes[i + 1])
                {
                    leftBorder = m_availableSizes[i];
                    rightBorder = m_availableSizes[i + 1];
                    break;
                }
            }

            float normalizeRightBorder = rightBorder - leftBorder;
            float normalizesizeMultiplier = sizeMultiplier - leftBorder;
            var percent = normalizesizeMultiplier / normalizeRightBorder;
            return (percent > 0.55f ? rightBorder : leftBorder);
        }

        public int GetSizeInDpi(int size)
        {
            return (int)(size * SizeMultiplier);
        }

    }

}
