using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MyTile", menuName = "2D/MyTile")]
public class MyTile : RuleTile<MyTile.Neighbor> {
    public int siblingGroup;
    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Sibing = 3;
    }
    public override bool RuleMatch(int neighbor, TileBase tile) {
        MyTile myTile = tile as MyTile;
        switch (neighbor) {
            case Neighbor.Sibing: return myTile && myTile.siblingGroup == siblingGroup;
        }
        return base.RuleMatch(neighbor, tile);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData){
        base.GetTileData(position, tilemap, ref tileData);
        // var iden = Matrix4x4.identity;
        //
        // tileData.sprite = m_DefaultSprite;
        // // tileData.color = m_Color;
        // tileData.gameObject = m_DefaultGameObject;
        // tileData.flags = TileFlags.LockTransform;
        // tileData.transform = iden;
    }
} 