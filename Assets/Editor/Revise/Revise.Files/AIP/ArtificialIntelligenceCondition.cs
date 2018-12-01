#region License

/**
 * Copyright (C) 2012 Jack Wakefield
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#endregion

using Revise.Files.AIP.Attributes;
using Revise.Files.AIP.Conditions;
namespace Revise.Files.AIP {
    /// <summary>
    /// Specifies the AI condition types.
    /// </summary>
    public enum ArtificialIntelligenceCondition {
        [ArtificialIntelligenceType(typeof(FightOrDelayCondition))]
        FightOrDelay = 1,
        
        [ArtificialIntelligenceType(typeof(CheckDamageCondition))]
        CheckDamage = 2,

        [ArtificialIntelligenceType(typeof(SelectNearestCharacterByTypeCondition))]
        SelectNearestCharacterByType = 3,

        [ArtificialIntelligenceType(typeof(CheckMoveDistanceCondition))]
        CheckMoveDistance = 4,

        [ArtificialIntelligenceType(typeof(CheckTargetDistanceCondition))]
        CheckTargetDistance = 5,

        [ArtificialIntelligenceType(typeof(CheckTargetAbilityCondition))]
        CheckTargetAbility = 6,

        [ArtificialIntelligenceType(typeof(CheckHealthCondition))]
        CheckHealth = 7,

        [ArtificialIntelligenceType(typeof(RandomCondition))]
        Random = 8,

        [ArtificialIntelligenceType(typeof(SelectNearestCharacterCondition))]
        SelectNearestCharacter = 9,

        [ArtificialIntelligenceType(typeof(HasTargetCondition))]
        HasTarget = 10,

        [ArtificialIntelligenceType(typeof(CompareAbilityCondition))]
        CompareAbility = 11,

        [ArtificialIntelligenceType(typeof(CheckAbilityCondition))]
        CheckAbility = 12,

        [ArtificialIntelligenceType(typeof(TimeOfDayCondition))]
        TimeOfDay = 13,

        [ArtificialIntelligenceType(typeof(CheckMagicStatusCondition))]
        CheckMagicStatus = 14,

        [ArtificialIntelligenceType(typeof(CheckNPCVariableCondition))]
        CheckNPCVariable = 15,

        [ArtificialIntelligenceType(typeof(CheckWorldVariableCondition))]
        CheckWorldVariable = 16,

        [ArtificialIntelligenceType(typeof(CheckEconomicVariableCondition))]
        CheckEconomicVariable = 17,

        [ArtificialIntelligenceType(typeof(SelectNPCCondition))]
        SelectNPC = 18,

        [ArtificialIntelligenceType(typeof(CheckDistanceBetweenOwnerCondition))]
        CheckDistanceBetweenOwner = 19,

        [ArtificialIntelligenceType(typeof(CheckZoneTimeCondition))]
        CheckZoneTime = 20,

        [ArtificialIntelligenceType(typeof(CheckSourceAbilityCondition))]
        CheckSourceAbility = 21,

        [ArtificialIntelligenceType(typeof(CheckOwnerCondition))]
        CheckOwner = 22,

        [ArtificialIntelligenceType(typeof(CheckOwnerTargetCondition))]
        CheckOwnerTarget = 23,

        [ArtificialIntelligenceType(typeof(CheckWorldTimeCondition))]
        CheckWorldTime = 24,

        [ArtificialIntelligenceType(typeof(CheckDateAndTimeCondition))]
        CheckDateAndTime = 25,

        [ArtificialIntelligenceType(typeof(CheckWeekDayAndTimeCondition))]
        CheckWeekDayAndTime = 26,

        [ArtificialIntelligenceType(typeof(CheckChannelNumberCondition))]
        CheckChannelNumber = 27,

        [ArtificialIntelligenceType(typeof(SelectNearestCharacterByCountCondition))]
        SelectNearestCharacterByCount = 28,

        [ArtificialIntelligenceType(typeof(CheckVariableCondition))]
        CheckVariable = 29,

        [ArtificialIntelligenceType(typeof(CheckTargetClanStatusCondition))]
        CheckTargetClanStatus = 30
    }
}