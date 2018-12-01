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

using Revise.Files.AIP.Actions;
using Revise.Files.AIP.Attributes;
namespace Revise.Files.AIP {
    /// <summary>
    /// Specifies the AI action types.
    /// </summary>
    public enum ArtificialIntelligenceAction {
        [ArtificialIntelligenceType(typeof(HaltActionsAction))]
        HaltActions = 1,

        [ArtificialIntelligenceType(typeof(PerformEmotionAction))]
        PerformEmotion = 2,

        [ArtificialIntelligenceType(typeof(SayMessageAction))]
        SayMessage = 3,

        [ArtificialIntelligenceType(typeof(MoveToRandomPositionAction))]
        MoveToRandomPosition = 4,

        [ArtificialIntelligenceType(typeof(MoveToRandomSpawnLocationAction))]
        MoveToRandomSpawnLocation = 5,

        [ArtificialIntelligenceType(typeof(MoveToRandomPositionOfSelectedCharacterAction))]
        MoveToRandomPositionOfSelectedCharacter = 6,

        [ArtificialIntelligenceType(typeof(MoveToNearbyCharacterByAbilityAction))]
        MoveToNearbyCharacterByAbility = 7,

        [ArtificialIntelligenceType(typeof(PerformSpecialAttackAction))]
        PerformSpecialAttack = 8,

        [ArtificialIntelligenceType(typeof(MoveAwayFromTargetAction))]
        MoveAwayFromTarget = 9,

        [ArtificialIntelligenceType(typeof(TransformIntoMonsterAction))]
        TransformIntoMonster = 10,

        [ArtificialIntelligenceType(typeof(SpawnMonsterAction))]
        SpawnMonster = 11,

        [ArtificialIntelligenceType(typeof(AttackTargetUsingNumberOfMonstersAction))]
        AttackTargetUsingNumberOfMonsters = 12,

        [ArtificialIntelligenceType(typeof(AttackNearestCharacterAction))]
        AttackNearestCharacter = 13,

        [ArtificialIntelligenceType(typeof(AttackSelectedCharacterAction))]
        AttackSelectedCharacter = 14,

        [ArtificialIntelligenceType(typeof(AttackTargetUsingAlliedMonstersAction))]
        AttackTargetUsingAlliedMonsters = 15,

        [ArtificialIntelligenceType(typeof(AttackTargetCharacterAction))]
        AttackTargetCharacter = 16,

        [ArtificialIntelligenceType(typeof(RunAwayAction))]
        RunAway = 17,

        [ArtificialIntelligenceType(typeof(DropRandomItemAction))]
        DropRandomItem = 18,

        [ArtificialIntelligenceType(typeof(AttackTargetUsingNumberOfMonstersAction))]
        AttackNearestCharacterUsingNumberOfMonsters = 19,

        [ArtificialIntelligenceType(typeof(AttackNearestCharacterDuplicateAction))]
        AttackNearestCharacterDuplicate = 20,

        [ArtificialIntelligenceType(typeof(SpawnMonsterAtPositionAction))]
        SpawnMonsterAtPosition = 21,

        [ArtificialIntelligenceType(typeof(Invalid22Action))]
        Invalid22 = 22,

        [ArtificialIntelligenceType(typeof(Invalid23Action))]
        Invalid23 = 23,

        [ArtificialIntelligenceType(typeof(CommitSuicideAction))]
        CommitSuicide = 24,

        [ArtificialIntelligenceType(typeof(PerformSkillAction))]
        PerformSkill = 25,

        [ArtificialIntelligenceType(typeof(SetNPCVariableAction))]
        SetNPCVariable = 26,

        [ArtificialIntelligenceType(typeof(SetWorldVariableAction))]
        SetWorldVariable = 27,

        [ArtificialIntelligenceType(typeof(SetEconomyVariableAction))]
        SetEconomyVariable = 28,

        [ArtificialIntelligenceType(typeof(SayMessageByTypeAction))]
        SayMessageByType = 29,

        [ArtificialIntelligenceType(typeof(MoveNearOwnerAction))]
        MoveNearOwner = 30,

        [ArtificialIntelligenceType(typeof(PerformTriggerAction))]
        PerformTrigger = 31,

        [ArtificialIntelligenceType(typeof(AttackOwnerIfNotAllyAction))]
        AttackOwnerIfNotAlly = 32,

        [ArtificialIntelligenceType(typeof(SetPlayerKillFlagAction))]
        SetPlayerKillFlag = 33,

        [ArtificialIntelligenceType(typeof(SetRegenerationSystemAction))]
        SetRegenerationSystem = 34,

        [ArtificialIntelligenceType(typeof(GiveItemToOwnerAction))]
        GiveItemToOwner = 35,

        [ArtificialIntelligenceType(typeof(SetAIVariableAction))]
        SetAIVariable = 36,

        [ArtificialIntelligenceType(typeof(SpawnMonsterWithMasterTypeAction))]
        SpawnMonsterWithMasterType = 37,

        [ArtificialIntelligenceType(typeof(SpawnMonsterAtPositionWithMasterTypeAction))]
        SpawnMonsterAtPositionWithMasterType = 38
    }
}