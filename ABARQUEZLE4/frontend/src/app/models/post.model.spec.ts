import { Post } from './post.model';
import '@types/jest';

describe('Post', () => {
    it('should create an instance', () => {
        const post: Post = {
            id: 1,
            title: 'Test Post',
            body: 'This is a test post.',
            dateCreated: new Date(),
            userName: 'testuser',
            firstName: 'John',
            lastName: 'Doe'
        };
        expect(post).toBeTruthy();
        expect(post.id).toEqual(1);
        // Add more assertions as needed
    });
});