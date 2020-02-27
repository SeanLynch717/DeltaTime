using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace deltaTime
{
    class ParticleManager
    {
        private Dictionary<ParticleSystem, double> particleSystems;
        private SpriteBatch sb { get; set; }
        private GameTime gt;

        public GameTime GT
        {
            set
            {
                gt = value;
            }
        }

        public ParticleManager(SpriteBatch sb)
        {
            this.particleSystems = new Dictionary<ParticleSystem, double>();
            this.sb = sb;
        }

        public void AddParticleSystem (ParticleSystem particleSystemToAdd, int lifetime)
        {
            particleSystems[particleSystemToAdd] = lifetime;
        }

        public void AddGenericParticleSystem(AnimatedTexture texture, Rectangle position)
        {
            ParticleSystem p = new ParticleSystem(new Vector2(0, 0.01f),
                                                  new Vector2(0.00f, -0.15f),
                                                  new Vector2(0.15f, 0), 64,
                                                  new Point(0, 0),
                                                  500,
                                                  EmmisionShape.Box,
                                                  1000,
                                                  50,
                                                  position.Location,
                                                  new Point(16, 16),
                                                  25,
                                                  texture);
            particleSystems[p] = 500;
        }
        public void RemoveAllParticles()
        {
            particleSystems.Clear();
        }

        public void Update ()
        {
            for(int i = 0; i < particleSystems.Keys.Count; i++)
            {
                ParticleSystem pS = particleSystems.Keys.ElementAt(i);
                if (particleSystems[pS] <= 0)
                {
                    particleSystems.Remove(pS);
                    i--;
                }
                else
                {
                    pS.Update(gt);
                    particleSystems[pS] -= gt.ElapsedGameTime.TotalMilliseconds;
                }
            }
        }

        public void Draw ()
        {
            foreach (ParticleSystem pS in particleSystems.Keys)
            {
                foreach (Particle p in pS.Particles)
                {
                    p.texture.Draw(new Rectangle(new Point((int)p.position.X, (int)p.position.Y), p.size), sb, gt, SpriteEffects.None);
                }
            }
        }
    }

    public enum EmmisionShape
    {
        Circle,
        Line,
        Box
    }

    class ParticleSystem
    {
        public List<Particle> Particles { get; set; }
        EmmisionShape eShape;
        Random ran = new Random();
        double timeElapsed = 0;

        public ParticleSystem(Vector2 forceOverTime, Vector2 initialVelocity, Vector2 initialVelocityRandom, int emmisionRadius, Point sizeOverTime, double particleLifeTime, EmmisionShape emmisionShape, double emmisionRate, int maxParticles, Point origin, Point particleStartingSize, int numInitialParticles, AnimatedTexture texture)
        {
            Particles = new List<Particle>();
            ran = new Random();
            this.forceOverTime = forceOverTime;
            this.initialVelocity = initialVelocity;
            this.initialVelocityRandom = initialVelocityRandom;
            this.emmisionRadius = emmisionRadius;
            this.sizeOverTime = sizeOverTime;
            this.particleLifeTime = particleLifeTime;
            this.emmisionRate = emmisionRate;
            this.maxParticles = maxParticles;
            this.origin = origin;
            this.particleStartingSize = particleStartingSize;
            this.texture = texture;
            eShape = emmisionShape;

            for(int i = 0; i < numInitialParticles; i++)
            {
                switch (eShape)
                {
                    case EmmisionShape.Circle:
                        break;
                    case EmmisionShape.Line:
                        Particles.Add(new Particle(new Vector2(origin.X + ran.Next(0, emmisionRadius), origin.Y), initialVelocity + (float)(ran.NextDouble() - 0.5) * initialVelocityRandom, texture, particleStartingSize));
                        break;
                    case EmmisionShape.Box:
                        Particles.Add(new Particle(new Vector2(origin.X + ran.Next(0, emmisionRadius), origin.Y + ran.Next(0, emmisionRadius)), initialVelocity + (float)(ran.NextDouble() - 0.5) * initialVelocityRandom, texture, particleStartingSize));
                        break;
                    default:
                        break;
                }
            }
        }

        //Particle System Properies
        public Vector2 forceOverTime { get; set; }
        public Vector2 initialVelocity { get; set; }
        public Vector2 initialVelocityRandom { get; set; }
        public int emmisionRadius { get; set; }
        public double startLifetime { get; set; }
        public Point particleStartingSize { get; set; }
        public Point sizeOverTime { get; set; }
        public double particleLifeTime { get; set; }
        public double emmisionRate { get; set; } //How many milliseconds before a new spawn
        public int maxParticles { get; set; }
        public Point origin { get; set; }

        //Particle Starting Properties
        public AnimatedTexture texture { get; set; }

        public void Update(GameTime gameTime)
        {
            //Spawning Particles
            if (timeElapsed <= emmisionRate)
            {
                timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            }
            else
            {
                //Console.WriteLine("Spawned Particle");
                switch (eShape)
                {
                    case EmmisionShape.Circle:
                        break;
                    case EmmisionShape.Line:
                        Particles.Add(new Particle(new Vector2(origin.X + ran.Next(0, emmisionRadius), origin.Y), initialVelocity + (float)(ran.NextDouble() - 0.5) * initialVelocityRandom, texture, particleStartingSize));
                        break;
                    case EmmisionShape.Box:
                        Particles.Add(new Particle(new Vector2(origin.X + ran.Next(0, emmisionRadius), origin.Y + ran.Next(0, emmisionRadius)), initialVelocity + (float)(ran.NextDouble() - 0.5) * initialVelocityRandom, texture, particleStartingSize));
                        break;
                    default:
                        break;
                }
                timeElapsed = 0;
            }
            
            //Updating the Particles
            if(Particles.Count != 0)
            {
                //foreach (Particle p in Particles)
                for(int i = 0; i < Particles.Count; i++)
                {
                    //Particle Pos and Size adjustments
                    Particles[i].position += Particles[i].velocity * gameTime.ElapsedGameTime.Milliseconds; // new Point(forceOverTime.X * gameTime.ElapsedGameTime.Milliseconds, forceOverTime.Y * gameTime.ElapsedGameTime.Milliseconds); //Might have to * everything by "gT.ElapsedGameTime.Milliseconds"
                    Particles[i].velocity += new Vector2(forceOverTime.X, forceOverTime.Y);
                    Particles[i].size += sizeOverTime;

                    //Particle Death
                    if (particleLifeTime < Particles[i].timeTillDeath)
                    {
                        Particles.Remove(Particles[i]);
                    }
                    else
                    {
                        Particles[i].timeTillDeath += 1;
                    }
                }
            }
        }
    }

    class Particle
    {
        public Particle(Vector2 position, Vector2 velocity, AnimatedTexture texture, Point size)
        {
            this.position = position;
            this.velocity = velocity;
            this.texture = texture;
            this.timeTillDeath = 0;
            this.size = size;
        }

        public Vector2 position { get; set; }
        public Vector2 velocity { get; set; }
        public AnimatedTexture texture { get; set; }
        public double timeTillDeath { get; set; }
        public Point size { get; set; }

    }
}
