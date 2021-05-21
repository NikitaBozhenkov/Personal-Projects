using UnityEngine;

public class ColorChanger {
    private const int ChangeValue = 20;

    private float[] m_colors;
    private int m_curChangeIndex;
    private int m_curChangeSign;

    public ColorChanger(int r, int g, int b, int curChangeIndex, int curChangeSign) {
        m_colors = new float[3]{ r, g, b};
        m_curChangeIndex = curChangeIndex;
        m_curChangeSign = curChangeSign;
    }

    public Color GetCurrentColor() {
        return new Color(m_colors[0] / 255, m_colors[1] / 255, m_colors[2] / 255);
    }

    public void ChangeColor() {
        m_colors[m_curChangeIndex] += ChangeValue * m_curChangeSign;

        if (m_colors[m_curChangeIndex] == 115 || m_colors[m_curChangeIndex] == 255) {
            m_curChangeSign *= -1;
            m_curChangeIndex = (m_curChangeIndex + 1) % 3;
        }
    }
}
