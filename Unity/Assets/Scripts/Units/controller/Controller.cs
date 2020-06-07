using System.Collections;
using System.Collections.Generic;
using AStar;
using Terrain;
using Units.utils;
using UnityEngine;

namespace Units
{
    public abstract class Controller
    {

        protected AbstractUnit body;
        protected float basisSpeed;
        protected float basisAttack;
        protected float basisDefense;
        
        protected List<EntityType> upgrades;
        protected bool isLastUpgarde;

        protected float deltaTime;
        protected Controller(AbstractUnit body)
        {
            this.body = body;
            deltaTime = 0.0f;
        }

        
        protected Stack<Tile> itineraire;
        protected int itineraireNumberRemain = 0;
            
        public abstract void interract(bool isRemoted, AbstractUnit target, Vector3 positionTarget);

        protected float getVitessUnit()
        {
            float baseVitess = UnitLibData.speed * Time.deltaTime * basisSpeed;

            Effect effect = body.GetEffect(0);

            int bonusLevel = effect.IdEffect == -1 ? 0 : effect.LevelEffect;


            return baseVitess + bonusLevel * baseVitess * 0.5f;
        }
        protected float getAttackUnit(AbstractUnit target)
        {
            float baseAttack = basisAttack;

            Effect effect = target.GetEffect(1); //defense

            int bonusLevel = effect.IdEffect == -1 ? 1 : effect.LevelEffect + 1;


            return basisAttack/(bonusLevel*target.brain.basisDefense);
        }

        public void calculatePath(Vector3 target)
        {
            if (TerrainMeshBuilder.alg != null && TerrainMeshBuilder.graph != null)
            {
                Vector3 exitPos = target;
                int xOffset = TerrainMeshBuilder.dimensions[0] / 2;
                int yOffset = TerrainMeshBuilder.dimensions[1] / 2;
                TerrainMeshBuilder.graph.BeginningNode = TerrainMeshBuilder.tiles[(int)body.GetPosition().x+xOffset, (int)body.GetPosition().z+yOffset];
                //Debug.Log("nodebeginpos="+TerrainMeshBuilder.graph.BeginningNode.Pos);
                TerrainMeshBuilder.graph.ExitNode = TerrainMeshBuilder.tiles[(int)(exitPos.x+xOffset), (int)(exitPos.z+yOffset)];
                //Debug.Log("nodeendpos="+TerrainMeshBuilder.graph.ExitNode.Pos);
                TerrainMeshBuilder.alg.Solve();
                itineraire = TerrainMeshBuilder.graph.ReconstructPath();
            }
        }

        public void updatePathMove()
        {
            
            int xOffset = TerrainMeshBuilder.dimensions[0] / 2;
            int yOffset = TerrainMeshBuilder.dimensions[1] / 2;
            Vector3 last = body.GetPosition();
            if (itineraire != null && itineraire.Count > itineraireNumberRemain)
            {
                Vector3 posTarget = itineraire.Peek().Pos; //- new Vector3(xOffset,0,yOffset);
                //Debug.Log(posTarget+"<->"+last);
                if (Vector3.Distance(last, posTarget) < 2f)
                {
                    
                    //Debug.Log(itineraire.Pop().Pos);
                } 
                body.SetPosition(Vector3.MoveTowards(last, posTarget, 5f * Time.deltaTime));
            }
        }
    }
}
