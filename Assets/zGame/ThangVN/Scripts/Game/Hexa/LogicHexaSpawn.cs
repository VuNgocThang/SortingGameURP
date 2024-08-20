using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicHexaSpawn : MonoBehaviour
{
    public hexa hexagonPrefab;
    public int rows = 5;
    public int columns = 5;

    void Start()
    {
        GenerateHexGrid();
    }

    void GenerateHexGrid()
    {
        //float xOffset = 1.732f; // Khoảng cách ngang giữa các ô (cos(30 độ) * 2)
        //float yOffset = 1.5f;   // Khoảng cách dọc giữa các ô (sin(30 độ) * 3)


        float xOffset = 1.0392f; // Khoảng cách ngang giữa các ô (cos(30 độ) * 2)
        float yOffset = 0.9f;   // Khoảng cách dọc giữa các ô (sin(30 độ) * 3)

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = new Vector3();
                position.x = col * xOffset;
                position.y = row * yOffset;
                position.z = 40f;

                // Điều chỉnh vị trí theo hàng chẵn và lẻ
                if (row % 2 == 1)
                    position.x += xOffset / 2;

                // Tạo hexagon mới tại vị trí tính toán
                hexa hexagon = Instantiate(hexagonPrefab, position, Quaternion.identity);
                hexagon.transform.SetParent(transform); // Đặt hexagon là con của Grid để giữ gọn gàng trong Scene

                // Cài đặt các thuộc tính khác cho hexagon nếu cần
                // Ví dụ: hexagon.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }

        //for (int row = 0; row < rows; row++)
        //{
        //    int hexCountInRow = (row % 2 == 0) ? rows : (rows - 1);

        //    for (int col = 0; col < hexCountInRow; col++)
        //    {
        //        Vector3 position = new Vector3();
        //        position.x = col * xOffset;
        //        position.y = row * yOffset;
        //        position.z = 40f;

        //        if (row % 2 == 1)
        //            position.x += xOffset / 2;

        //        hexa hexagon = Instantiate(hexagonPrefab, transform);
        //        hexagon.Init(row, col);
        //        hexagon.transform.position = position;
        //    }
        //}
    }
}
