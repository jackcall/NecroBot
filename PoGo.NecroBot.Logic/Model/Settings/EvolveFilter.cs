using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using POGOProtos.Enums;
using System;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "", ItemRequired = Required.DisallowNull)] //Dont set Title
    public class EvolveFilter
    {
        public EvolveFilter()
        {
            Moves = new List<List<PokemonMove>>();
            EnableEvolve = true;
            Operator = "or";
        }


        public EvolveFilter(int evolveIV, int evolveLV, int minCP, string evoOperator = "and", string evolveTo = "", List<List<PokemonMove>> moves = null)
        {
            this.Moves = new List<List<PokemonMove>>();
            if (moves != null) this.Moves = moves;
            EnableEvolve = true;
            this.MinIV = evolveIV;
            this.MinLV = evolveLV;
            this.EvolveTo = evolveTo;
            this.MinCP = MinCP;
            this.Operator = evoOperator;
        }

        [NecrobotConfig(IsPrimaryKey = true, Key = "Enable Envolve", Description = "Allow bot auto evolve this pokemon", Position = 1)]
        [DefaultValue(false)]
        [JsonIgnore]
        public bool EnableEvolve { get; set; }

        [NecrobotConfig(Key = "Evolve Min IV", Description = "Min IV for auto evolve", Position = 2)]
        [DefaultValue(95)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public int MinIV { get; set; }

        [NecrobotConfig(Key = "Evolve Min LV", Description = "Min LV for auto evolve", Position = 3)]
        [DefaultValue(95)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public int MinLV { get; set; }

        [NecrobotConfig(Key = "Evolve Min CP", Description = "Min CP for auto evolve", Position = 4)]
        [DefaultValue(10)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public int MinCP { get; set; }

        [NecrobotConfig(Key = "Moves", Description = "Define list of desire move for evolve", Position = 5)]
        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public List<List<PokemonMove>> Moves { get; set; }

        [NecrobotConfig(Key = "Operator", Position = 6, Description = "The operator logic use to check for evolve")]
        [DefaultValue("or")]
        [EnumDataType(typeof(Operator))]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 5)]
        public string Operator { get; set; }

        [NecrobotConfig(Key = "Evolve To", Position = 6, Description = "Select branch to envolve to for multiple branch pokemon like Poliwirl")]
        [DefaultValue("")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 6)]
        public string EvolveTo { get; set; }

        [JsonIgnore]
        public PokemonId EvolveToPokemonId
        {
            get
            {
                PokemonId id = PokemonId.Missingno;

                if (Enum.TryParse<PokemonId>(this.EvolveTo, out id))
                {
                    return id;
                }

                return id;
            }
        }
        internal static Dictionary<PokemonId, EvolveFilter> Default()
        {
            return new Dictionary<PokemonId, EvolveFilter>
            {
                {PokemonId.Rattata, new EvolveFilter(0, 0, 0, "or")},
                {PokemonId.Zubat, new EvolveFilter(0, 0, 0, "or")},
                {PokemonId.Pidgey, new EvolveFilter(0, 0, 0, "or")},
                {PokemonId.Caterpie, new EvolveFilter(0, 0, 0,  "or") },
                {PokemonId.Weedle, new EvolveFilter(0, 0, 0,  "or") },

                {PokemonId.Porygon, new EvolveFilter(100, 28, 500, "and",PokemonId.Porygon2.ToString())},
                {PokemonId.Gloom , new EvolveFilter(100, 28, 500, "and",PokemonId.Bellossom.ToString())} ,
                {PokemonId.Sunkern , new EvolveFilter(100, 28, 500, "and",PokemonId.Sunflora.ToString())}  ,
                {PokemonId.Slowpoke, new EvolveFilter(100, 28, 500, "and",PokemonId.Slowking.ToString())},
                {PokemonId.Poliwhirl , new EvolveFilter(100, 28, 500, "and",PokemonId.Politoed.ToString())},
                {PokemonId.Seadra , new EvolveFilter(100, 28, 500, "and",PokemonId.Kingdra.ToString())},
                {PokemonId.Dratini, new EvolveFilter(100, 30, 800, "and")}

            };
        }
    }
}