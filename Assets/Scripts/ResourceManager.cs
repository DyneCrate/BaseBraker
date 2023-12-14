using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TODO Save Scriptable Objects in Lists to choose from
 * 
 * 
 */

public class ResourceManager : Singleton<ResourceManager>
{
    public List<TerrainType> TerrainTypes = new List<TerrainType>();
}
