/**
 * Represents a mapping where keys are strings and values are of type T.
 * @template T The type of values associated with keys in the mapping.
 */
export type StringMapping<T> = {
    [key: string]: T;
};