using JetBrains.Annotations;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Data
{
    public static Work work = new Work();

    /// <summary>
    /// Propagates an id change on `changed` to every other Block (in the
    /// template list and in all cards) that shares the same `uid`, so that
    /// renaming a block keeps all its copies in sync.
    /// </summary>
    public static void SyncBlockId(Block changed)
    {
        foreach (var block in work.blocks)
            if (block != changed && block.uid == changed.uid)
                block.id = changed.id;

        foreach (var card in work.cards)
            foreach (var block in card.blocks)
                if (block != changed && block.uid == changed.uid)
                    block.id = changed.id;
    }

    /// <summary>
    /// Propagates the operand list shape (count, order, type) of `changed` to
    /// every other Block (in the template list and in all cards) that shares
    /// the same `uid`. Operands are matched across copies by `uid`: matched
    /// operands keep their own `num`/`linkedBlock` data (only `type` is
    /// synced), unmatched ones are dropped, and new operands from `changed`
    /// are added with default values.
    /// </summary>
    public static void SyncBlockOperands(Block changed)
    {
        foreach (var block in work.blocks)
            if (block != changed && block.uid == changed.uid)
                SyncOperandList(changed, block);

        foreach (var card in work.cards)
            foreach (var block in card.blocks)
                if (block != changed && block.uid == changed.uid)
                    SyncOperandList(changed, block);
    }

    private static void SyncOperandList(Block source, Block target)
    {
        List<Operand> synced = new List<Operand>();
        foreach (var srcOp in source.operands)
        {
            Operand existing = target.operands.Find(o => o.uid == srcOp.uid);
            if (existing != null)
            {
                existing.type = srcOp.type;
                synced.Add(existing);
            }
            else
            {
                Operand copy = new Operand();
                copy.uid = srcOp.uid;
                copy.type = srcOp.type;
                synced.Add(copy);
            }
        }
        target.operands = synced;
    }
}

[System.Serializable]
public class Work {
    public string json = "";
    public string export = "c://";
    public List<Block> blocks = new List<Block>();
    public List<Card> cards = new List<Card>();

    /// <summary>Call before serializing to JSON.</summary>
    public void PrepareForSave()
    {
        foreach (var card in cards)
            card.PrepareForSave();
    }

    /// <summary>Call after deserializing from JSON.</summary>
    public void ResolveLinks()
    {
        foreach (var card in cards)
            card.ResolveLinks();
    }
}

[System.Serializable]
public class Card
{
    public List<Block> blocks = new List<Block>();
    public List<Attr> attrs = new List<Attr>();
    public string id;

    public static Card NewCard() { 
        Card c = new Card();
        c.id = "newCard";
        return c;
    }

    /// <summary>
    /// Call before serializing: turns each operand's `linkedBlock` reference
    /// into an index (`linkedBlockIndex`) into this card's `blocks` list.
    /// </summary>
    public void PrepareForSave()
    {
        foreach (var block in blocks)
            foreach (var operand in block.operands)
                operand.linkedBlockIndex = operand.linkedBlock != null ? blocks.IndexOf(operand.linkedBlock) : -1;
    }

    /// <summary>
    /// Call after deserializing: rebuilds each operand's `linkedBlock` reference
    /// from its saved `linkedBlockIndex`.
    /// </summary>
    public void ResolveLinks()
    {
        foreach (var block in blocks)
            foreach (var operand in block.operands)
                operand.linkedBlock = (operand.linkedBlockIndex >= 0 && operand.linkedBlockIndex < blocks.Count)
                    ? blocks[operand.linkedBlockIndex]
                    : null;
    }

    /// <summary>
    /// Returns the same set of blocks as `blocks`, but with the "root" blocks
    /// (those not referenced by any operand's linkedBlock) placed first.
    /// </summary>
    public List<Block> GetOrderedBlocks()
    {
        HashSet<Block> referenced = new HashSet<Block>();
        foreach (var block in blocks)
            foreach (var operand in block.operands)
                if (operand.linkedBlock != null)
                    referenced.Add(operand.linkedBlock);

        List<Block> roots = new List<Block>();
        List<Block> rest = new List<Block>();
        foreach (var block in blocks)
        {
            if (referenced.Contains(block))
                rest.Add(block);
            else
                roots.Add(block);
        }

        roots.AddRange(rest);
        return roots;
    }
}

[System.Serializable]
public class Attr {
    public string name;
    public string value;
}

[System.Serializable]
public class Block
{
    public List<Operand> operands = new List<Operand>();
    public string id = "new block";
    public string uid = System.Guid.NewGuid().ToString();
    public float x = 623;
    public float y = 178;

    public static Block NewBlock()
    {
        Block block = new Block();
        return block;
    }

    public Block Copy() {
        Block block = new Block();
        block.id = id;
        block.uid = uid;
        foreach (Operand operand in operands)
        {
            Operand operand1 = new Operand();
            operand1.uid = operand.uid;
            operand1.type = operand.type;
            block.operands.Add(operand1);
        }
        return block;
    }
}

[System.Serializable]
public class Operand {
    // Not serialized directly: a direct object reference would make JsonUtility
    // either duplicate the linked block or break on the resulting reference cycle.
    // The actual link is persisted via `linkedBlockIndex` (see Card.PrepareForSave /
    // Card.ResolveLinks) and this field is rebuilt after loading.
    [System.NonSerialized]
    public Block linkedBlock = null;
    public int linkedBlockIndex = -1;

    public string uid = System.Guid.NewGuid().ToString();
    public OperandType type = OperandType.Action;
    public string data = "";
}

public enum OperandType { 
    Action,
    PayItem,
    Condition,
    String
}