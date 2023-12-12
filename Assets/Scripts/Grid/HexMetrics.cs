using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public static float OuterRadius (float hexSize)
    {
        return hexSize;
    }

    public static float InnerRadius (float hexSize)
    {
        return hexSize * 0.866025404f;
    }

    public static Vector3[] Corners(float hexSize, HexOrientation orientation)
    {
        Vector3[] corners = new Vector3[6];
        for (int i = 0; i < 6; i++) 
        {
            corners[i] = Corner(hexSize, orientation, i);
        }
        return corners;
    }

    public static Vector3 Corner(float hexSize, HexOrientation orientation, int index)
    {
        float angle = 60f * index;
        if (orientation == HexOrientation.PointyTop) 
        {
            angle += 30f;
        }
        Vector3 corner = new Vector3(hexSize * Mathf.Cos(angle * Mathf.Deg2Rad),
            0f,
            hexSize * Mathf.Sin(angle * Mathf.Deg2Rad)
            );
        return corner;
    }

    public static Vector3 Center(float hexSize, int x, int z, HexOrientation orientation)
    {
        Vector3 centrePosition;
        if (orientation == HexOrientation.PointyTop)
        {
            centrePosition.x = (x + z * 0.5f - z / 2) * (InnerRadius(hexSize) * 2f);
            centrePosition.y = 0f;
            centrePosition.z = z * (OuterRadius(hexSize) * 1.5f);
        }
        else
        {
            centrePosition.x = x * (OuterRadius(hexSize) * 1.5f);
            centrePosition.y = 0f;
            centrePosition.z = (z + x * 0.5f - x / 2) * (InnerRadius(hexSize) * 2f);
        }
        return centrePosition;
    }

    public static Vector3 OffsetToCube(Vector2 offsetCoord, HexOrientation orientation)
    {
        return OffsetToCube((int)offsetCoord.x, (int)offsetCoord.y, orientation);
    }
    public static Vector3 OffsetToCube(int col, int row, HexOrientation orientation)
    {
        if (orientation == HexOrientation.PointyTop)
        {
            return AxialToCube(OffsetToAxialPointy(col, row));
        }
        else
        {
            return AxialToCube(OffsetToAxialFlat(col, row));
        }
    }
    public static Vector3 AxialToCube(Vector2Int axial)
    {
        float x = axial.x;
        float z = axial.y;
        float y = -x - z;
        return new Vector3(x, z, y);
    }
    public static Vector2Int OffsetToAxialFlat(int col, int row)
    {
        int q = col;
        int r = row - (col + (col & 1)) / 2;
        return new Vector2Int(q, r);
    }

    public static Vector2Int OffsetToAxialPointy(int col, int row)
    {
        int q = col - (row + (row & 1)) / 2;
        int r = row;
        return new Vector2Int(q, r);
    }

    public static Vector2 CubeToAxial(Vector3 cube)
    {
        return new Vector2(cube.x, cube.y);
    }

    public static Vector2 OffsetToAxial(int x, int z, HexOrientation orientation)
    {
        if (orientation == HexOrientation.PointyTop)
        {
            return OffsetToAxialPointy(x,z);
        }
        else
        {
            return OffsetToAxialFlat(x,z);
        }
    }

    public static Vector2 CubeToOffset(int x, int y, int z, HexOrientation orientation)
    {
        if (orientation == HexOrientation.PointyTop)
        {
            return CubeToOffsetPointy(x, y, z);
        }
        else
        {
            return CubeToOffsetFlat(x, y, z);
        }
    }

    public static Vector2 CubeToOffset(Vector3 offsetCoord, HexOrientation orientation)
    {
        return CubeToOffset((int)offsetCoord.x, (int)offsetCoord.y, (int)offsetCoord.z, orientation);
    }

    private static Vector2 CubeToOffsetPointy(int x, int y, int z)
    {
        Vector2 offsetCoordinates = new Vector2(x + (y - (y & 1)) / 2, y);
        return offsetCoordinates;
    }

    private static Vector2 CubeToOffsetFlat(int x, int y, int z)
    {
        Vector2 offsetCoordinates = new Vector2(x, y + (x - (x & 1)) / 2);
        return offsetCoordinates;
    }

    private static Vector3 CubeRound(Vector3 frac)
    {
        Vector3 roundedCoordinates = new();
        int rx = Mathf.RoundToInt(frac.x);
        int ry = Mathf.RoundToInt(frac.y);
        int rz = Mathf.RoundToInt(frac.z);
        float xDiff = Mathf.Abs(rx - frac.x);
        float yDiff = Mathf.Abs(ry - frac.y);
        float zDiff = Mathf.Abs(rz - frac.z);

        if (xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry - rz;
        }
        else if (yDiff > zDiff)
        { 
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }
        roundedCoordinates.x = rx;
        roundedCoordinates.y = ry;
        roundedCoordinates.z = rz;
        return roundedCoordinates;
    }

    public static Vector2 AxialRound(Vector2 coordinates)
    {
        return CubeToAxial(CubeRound(AxialToCube(coordinates.x, coordinates.y)));
    }
    public static Vector2 CoordinateToAxial(float x, float z, float hexSize, HexOrientation orientation)
    {
        if (orientation == HexOrientation.PointyTop)
        {
            return CoordinateToPointyAxial(x, z, hexSize);
        }
        else
        {
            return CoordinateToFlatAxial(z, x, hexSize);
        }
    }

    private static Vector2 CoordinateToPointyAxial(float x, float z, float hexSize)
    {
        Vector2 pointyHexCoordinates = new();
        pointyHexCoordinates.x = (Mathf.Sqrt(3) / 3 * x - 1f / 3 * z) / hexSize;
        pointyHexCoordinates.y = (2f / 3 * z) / hexSize;

        return AxialRound(pointyHexCoordinates);
    }

    private static Vector2 CoordinateToFlatAxial(float x, float z, float hexSize)
    {
        Vector2 flatHexCoordinates = new();
        flatHexCoordinates.x = (2f / 3 * x) / hexSize;
        flatHexCoordinates.y = (-1f / 3 * x + MathF.Sqrt(3) / 3 * z) / hexSize;

        return AxialRound(flatHexCoordinates);
    }

    public static Vector2 CoordinateToOffset(float x, float z, float hexSize, HexOrientation orientation)
    {
        return CubeToOffset(AxialToCube(CoordinateToAxial(x, z, hexSize, orientation)), orientation);
    }

    public static Vector3 AxialToCube(int q, int r)
    {
        return new Vector3(q, r, -q - r);
    }
    public static Vector3 AxialToCube(float q, float r)
    {
        return new Vector3(q, r, -q - r);
    }
    public static Vector3 AxialToCube(Vector2 axialCoord)
    {
        return AxialToCube(axialCoord.x, axialCoord.y);
    }
}
