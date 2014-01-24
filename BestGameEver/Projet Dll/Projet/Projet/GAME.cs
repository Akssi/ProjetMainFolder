using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TOOLS;

namespace Projet
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GAME : Microsoft.Xna.Framework.Game
    {
       const float INTERVALLE_CALCUL_FPS = 1f;
      const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
      GraphicsDeviceManager P�riph�riqueGraphique { get; set; }
      SpriteBatch GestionSprites { get; set; }

      RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
      RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
      RessourcesManager<Model> GestionnaireDeMod�les { get; set; }
      RessourcesManager<Effect> GestionnaireDeShaders { get; set; }
      Cam�ra Cam�raJeu { get; set; }

      public InputManager GestionInput { get; private set; }

      public GAME()
      {
         P�riph�riqueGraphique = new GraphicsDeviceManager(this);
         Content.RootDirectory = "Content";
         P�riph�riqueGraphique.SynchronizeWithVerticalRetrace = false;
         IsFixedTimeStep = false;
         IsMouseVisible = true;
      }

      protected override void Initialize()
      {
         const int DIMENSION_TERRAIN = 256;
         Vector2 �tenduePlan = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN);
         Vector2 charpentePlan = new Vector2(4, 3);
         Vector3 positionCam�ra = new Vector3(0, 250, 125);
         Vector3 cibleCam�ra = new Vector3(0, 0, 0);
         Vector3 positionPhare = new Vector3(0, 0, 0);
         Vector3 positionAvion1 = new Vector3(30, 20, 0);
         Vector3 positionAvion2 = new Vector3(20, 15, 5);
         Vector3 positionVaisseau = new Vector3(0, 25, 25);

         GestionnaireDeFonts = new RessourcesManager<SpriteFont>(this, "Fonts");
         GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
         GestionnaireDeMod�les = new RessourcesManager<Model>(this, "Models");
         GestionnaireDeShaders = new RessourcesManager<Effect>(this, "Effects"); 
         GestionInput = new InputManager(this);
         Cam�raJeu = new Cam�raSubjective(this, positionCam�ra, cibleCam�ra, INTERVALLE_MAJ_STANDARD);

         Components.Add(GestionInput);
         Components.Add(Cam�raJeu);
         Components.Add(new Afficheur3D(this));
         Components.Add(new Terrain(this, 1f, Vector3.Zero, Vector3.Zero, new Vector3(DIMENSION_TERRAIN, 50, DIMENSION_TERRAIN), "CarteAS", "D�tailsTerrain", 5, INTERVALLE_MAJ_STANDARD));
         Components.Add(new PlanTextur�(this, 1f, new Vector3(0, MathHelper.PiOver2, 0), new Vector3(-DIMENSION_TERRAIN / 2, DIMENSION_TERRAIN / 2, 0), �tenduePlan, charpentePlan, "CielGauche", INTERVALLE_MAJ_STANDARD));
         Components.Add(new PlanTextur�(this, 1f, new Vector3(0, -MathHelper.PiOver2, 0), new Vector3(DIMENSION_TERRAIN / 2, DIMENSION_TERRAIN / 2, 0), �tenduePlan, charpentePlan, "CielDroite", INTERVALLE_MAJ_STANDARD));
         Components.Add(new PlanTextur�(this, 1f, Vector3.Zero, new Vector3(0, DIMENSION_TERRAIN / 2, -DIMENSION_TERRAIN / 2), �tenduePlan, charpentePlan, "CielAvant", INTERVALLE_MAJ_STANDARD));
         Components.Add(new PlanTextur�(this, 1f, new Vector3(0, -MathHelper.Pi, 0), new Vector3(0, DIMENSION_TERRAIN / 2, DIMENSION_TERRAIN / 2), �tenduePlan, charpentePlan, "CielArri�re", INTERVALLE_MAJ_STANDARD));
         Components.Add(new PlanTextur�(this, 1f, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, DIMENSION_TERRAIN-1, 0), �tenduePlan, charpentePlan, "CielDessus", INTERVALLE_MAJ_STANDARD));
         Components.Add(new ObjetDeBase(this, "Phare", 0.10f, Vector3.Zero, Vector3.Zero));
         Components.Add(new AfficheurFPS(this, "Arial20", INTERVALLE_CALCUL_FPS));

         Services.AddService(typeof(Random), new Random());
         Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
         Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
         Services.AddService(typeof(RessourcesManager<Model>), GestionnaireDeMod�les);
         Services.AddService(typeof(RessourcesManager<Effect>), GestionnaireDeShaders);
         Services.AddService(typeof(InputManager), GestionInput);
         Services.AddService(typeof(Camera), Cam�raJeu);
         GestionSprites = new SpriteBatch(GraphicsDevice);
         Services.AddService(typeof(SpriteBatch), GestionSprites);
         base.Initialize();
      }

      protected override void Update(GameTime gameTime)
      {
         G�rerClavier();
         base.Update(gameTime);
      }

      private void G�rerClavier()
      {
         Vector3 positionCam�ra0 = new Vector3(0, 250, 125);
         Vector3 positionCam�ra1 = new Vector3(0, 200, 0F);
         Vector3 positionCam�ra2 = new Vector3(0, 70, 50);
         Vector3 positionCam�ra3 = new Vector3(0, 15, 50);
         Vector3 positionCam�ra4 = new Vector3(8, 26, 30);
         Vector3 cibleCam�ra = new Vector3(0, 0, 0);
         if (GestionInput.EstEnfonc�e(Keys.Escape))
         {
            Exit();
         }
         if (!(GestionInput.EstEnfonc�e(Keys.LeftShift) || GestionInput.EstEnfonc�e(Keys.RightShift) ||
               GestionInput.EstEnfonc�e(Keys.LeftControl) || GestionInput.EstEnfonc�e(Keys.RightControl)))
         {
            if (GestionInput.EstNouvelleTouche(Keys.D0) || GestionInput.EstNouvelleTouche(Keys.NumPad0))
            {
               Cam�raJeu.D�placer(positionCam�ra0, cibleCam�ra, Vector3.Up);
            }
            else
            {
               if (GestionInput.EstNouvelleTouche(Keys.D1) || GestionInput.EstNouvelleTouche(Keys.NumPad1))
               {
                  Cam�raJeu.D�placer(positionCam�ra1, cibleCam�ra, Vector3.Forward);
               }
               else
               {
                  if (GestionInput.EstNouvelleTouche(Keys.D2) || GestionInput.EstNouvelleTouche(Keys.NumPad2))
                  {
                     Cam�raJeu.D�placer(positionCam�ra2, cibleCam�ra+Vector3.Forward*10, Vector3.Up);
                  }
                  else
                  {
                     if (GestionInput.EstNouvelleTouche(Keys.D3) || GestionInput.EstNouvelleTouche(Keys.NumPad3))
                     {
                        Cam�raJeu.D�placer(positionCam�ra3, cibleCam�ra + Vector3.Forward * 10 + Vector3.Up * 20, Vector3.Up);
                     }
                     else
                     {
                        if (GestionInput.EstNouvelleTouche(Keys.D4) || GestionInput.EstNouvelleTouche(Keys.NumPad4))
                        {
                           Cam�raJeu.D�placer(positionCam�ra4, positionCam�ra4 + Vector3.Forward * 10, Vector3.Up);
                        }
                     }
                  }
               }
            }
         }
      }

      protected override void Draw(GameTime gameTime)
      {
         GraphicsDevice.Clear(Color.DarkBlue);
         base.Draw(gameTime);
      }
    }
}
