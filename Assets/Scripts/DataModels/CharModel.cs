using UnityEngine;
using System.Collections;
using JsonFx.Json;

namespace UnityRose
{
    [JsonOptIn]
	public class Stats {
        [JsonMember]
        public float atk { get; set; }
        [JsonMember]
        public float def { get; set; }
        [JsonMember]
        public float dex { get; set; }
        [JsonMember]
        public float intel { get; set; }
        [JsonMember]
        public float crit { get; set; }
        [JsonMember]
        public float luck { get; set; }
        [JsonMember]
        public float movSpd { get; set; }
        [JsonMember]
        public float atkSpd { get; set; }

		public Stats()
		{
			atk = 10;
			def = 10;
			dex = 10;
			intel = 10;
			crit = 10;
			luck = 10;
			movSpd = 10;
			atkSpd = 10;
		}
	}

    [JsonOptIn]
	public class Equip {
        [JsonMember]
        public int faceID { get; set; }
        [JsonMember]
        public int hairID { get; set; }
        [JsonMember]
        public int chestID { get; set; }
        [JsonMember]
        public int footID { get; set; }
        [JsonMember]
        public int handID { get; set; }
        [JsonMember]
        public int weaponID { get; set; }
        [JsonMember]
        public int shieldID { get; set; }
        [JsonMember]
        public int backID { get; set; }
        [JsonMember]
        public int maskID { get; set; }
        [JsonMember]
        public int capID { get; set; }

		public Equip(){	
			faceID = 1;
			hairID = 0;
			chestID = 0;
			footID = 0;
			handID = 0;
			weaponID = 0;
			shieldID = 0;
			backID = 0;
			maskID = 0;
			capID = 0;
		}
		
		public Equip(int chest, int foot, int hand, int cap, int weapon, int shield, int face, int hair,  int back, int mask)
		{
			chestID = chest;
			footID = foot;
			handID = hand;
			capID = cap;
			weaponID = weapon;
			shieldID = shield;
			faceID = face;
			hairID = hair;
			backID = back;
			maskID = mask;
			
		}
	}

    [JsonOptIn]
	public class CharModel {
        [JsonMember]
		public string name { get; set; }
        [JsonMember]
        public string map { get; set; }
        [JsonMember]
        public Job1Type job1 { get; set; }
        [JsonMember]
        public Job2Type job2 { get; set; }
        [JsonMember]
        public int level { get; set; }
        [JsonMember]
        public Vector3 pos { get; set; }
        [JsonMember]
        public GenderType gender { get; set; }
        [JsonMember]
        public WeaponType weapon { get; set; }
        [JsonMember]
        public RigType rig { get; set; }
        [JsonMember]
        public States state { get; set; }
        [JsonMember]
        public Stats stats { get; set; }
        [JsonMember]
        public Equip equip { get; set; }

		public CharModel()
		{
			LoadModel ("New", GenderType.MALE, WeaponType.EMPTY, Job1Type.VISITOR, Job2Type.NONE, 1, new Vector3( 0, 0 ,0), new Stats(), new Equip());
		}

		public CharModel(string name)
		{
			LoadModel (name, GenderType.MALE, WeaponType.EMPTY, Job1Type.VISITOR, Job2Type.NONE, 1, new Vector3( 0, 0 ,0), new Stats(), new Equip());
		}

		public CharModel(GenderType gender)
		{
			LoadModel ("New", gender, WeaponType.EMPTY, Job1Type.VISITOR, Job2Type.NONE, 1, new Vector3( 0, 0 ,0), new Stats(), new Equip());
		}

		public CharModel(GenderType gender, WeaponType weapon)
		{
			LoadModel ("New", gender, weapon, Job1Type.VISITOR, Job2Type.NONE, 1, new Vector3( 0, 0 ,0), new Stats(), new Equip());
		}

		public CharModel(string name, GenderType gender)
		{
			LoadModel (name, gender, WeaponType.EMPTY, Job1Type.VISITOR, Job2Type.NONE, 1, new Vector3( 0, 0 ,0), new Stats(), new Equip());
		}

		public CharModel(string name, GenderType gender, WeaponType weapon)
		{
			LoadModel (name, gender, weapon, Job1Type.VISITOR, Job2Type.NONE, 1, new Vector3( 0, 0 ,0), new Stats(), new Equip());
		}

		public CharModel(string name, GenderType gender, WeaponType weapon,  Job1Type job1, Job2Type job2, int level)
		{
			LoadModel (name, gender, weapon, job1, job2, level, new Vector3( 0, 0 ,0), new Stats(), new Equip());
		}

		public CharModel(string name, GenderType gender, WeaponType weapon, Job1Type job1, Job2Type job2, int level, Vector3 pos)
		{
			LoadModel (name, gender, weapon, job1, job2, level, pos, new Stats(), new Equip());
		}

		public CharModel(string name, GenderType gender, WeaponType weapon, Job1Type job1, Job2Type job2, int level, Vector3 pos, Stats stats)
		{
			LoadModel (name, gender, weapon, job1, job2, level, pos, stats, new Equip());
		}

		public void LoadModel(string name, GenderType gender, WeaponType weapon, Job1Type job1, Job2Type job2, int level, Vector3 pos, Stats stats, Equip equip)
		{
			this.name = name;
			this.gender = gender;
			this.weapon = weapon;
			this.job1 = job1;
			this.job2 = job2;
			this.level = level;
			this.pos = pos;
			this.stats = stats;
			this.equip = equip;
            this.rig = RigType.FOOT;
            this.state = States.STANDING;
            this.map = "zantCity"; // TODO: change to the default new player map
		}

		public void changeID(BodyPartType bodyPart, int id)
		{
			switch (bodyPart) {
				case BodyPartType.ARMS:
					equip.handID = id; break;
				case BodyPartType.BACK:
					equip.backID = id; break;
				case BodyPartType.BODY:
					equip.chestID = id; break;
				case BodyPartType.CAP:
					equip.capID = id; break;
				case BodyPartType.FACE:
					equip.faceID = id; break;
				case BodyPartType.FACEITEM:
					equip.maskID = id; break;
				case BodyPartType.FOOT:
					equip.footID = id; break;
				case BodyPartType.HAIR:
					equip.hairID = id; break;
				case BodyPartType.SUBWEAPON:
					equip.shieldID = id; break;
				case BodyPartType.WEAPON:
					equip.weaponID = id; break;
				default:
					break;
			}
		}

	}
}
